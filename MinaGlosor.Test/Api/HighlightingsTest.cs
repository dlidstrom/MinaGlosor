using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using NUnit.Framework;
using Raven.Abstractions.Data;
using Raven.Abstractions.Indexing;
using Raven.Client;
using Raven.Client.Embedded;
using Raven.Client.Indexes;

namespace MinaGlosor.Test.Api
{
    [TestFixture(Ignore = true, IgnoreReason = "Code spike")]
    public class HighlightingsTest
    {
        private IDocumentStore store;

        [SetUp]
        public void SetUp()
        {
            store = new EmbeddableDocumentStore
                {
                    RunInMemory = true
                }.Initialize();
            IndexCreation.CreateIndexes(Assembly.GetExecutingAssembly(), store);
            Transact(session => session.Store(new Document { Text = "The quick brown fox jumped over the lazy dog" }));
        }

        [TearDown]
        public void TearDown()
        {
            store.Dispose();
        }

        [Test]
        public void CanHighlight()
        {
            Transact(session =>
            {
                FieldHighlightings highlightings = null;
                var results = session.Query<Document, DocumentIndex>()
                                     .Customize(x => x.Highlight("Text", 128, 1, out highlightings))
                                     .Search(x => x.Text, "brown")
                                     .ProjectFromIndexFieldsInto<DocumentViewModel>()
                                     .Take(1)
                                     .ToArray();
                Assert.That(results, Has.Length.EqualTo(1), "Expected 1 result");
                Assert.That(highlightings.ResultIndents.ToArray(), Has.Length.EqualTo(1), "Expected 1 highlighting");
            });
        }

        private void Transact(Action<IDocumentSession> action)
        {
            using (var session = store.OpenSession())
            {
                action.Invoke(session);
                session.SaveChanges();
            }

            WaitForIndexing();
        }

        private void WaitForIndexing()
        {
            const int Timeout = 15000;
            var indexingTask = Task.Factory.StartNew(
                () =>
                {
                    var sw = Stopwatch.StartNew();
                    while (sw.Elapsed.TotalMilliseconds < Timeout)
                    {
                        var s = store.DatabaseCommands.GetStatistics()
                            .StaleIndexes;
                        if (s.Length == 0)
                        {
                            break;
                        }

                        Task.Delay(500).Wait();
                    }
                });
            indexingTask.Wait(Timeout);
        }

        public class Document
        {
            public string Text { get; set; }

            public string Id { get; set; }
        }

        public class DocumentViewModel
        {
            public string Text { get; set; }

            public string Id { get; set; }
        }

        public class DocumentIndex : AbstractIndexCreationTask<Document>
        {
            public DocumentIndex()
            {
                Map = documents => from document in documents
                                   select new
                                   {
                                       document.Text
                                   };

                /*
                 * this blog gives a decent conceptual idea of what is going on:
                 * http://blog.jpountz.net/post/41301889664/putting-term-vectors-on-a-diet
                 *
                 * TermVector.Yes = store an inverted index per document with terms and their frequencies within that document
                 * TermVector.WithPositions = also include the positions of the terms relative to other terms (tokens)
                 * TermVector.WithOffsets = also include the locations of the term in the original field text
                 * TermVector.WithPositionsAndOffsets = store an inverted term index per document including term frequencies, positions and offsets per document
                 *
                 * For Highlighting, you must use TermVector.WithPositionsAndOffsets because the FastVectorHighlighter needs to know
                 * the relative positions of tokens and where they are located in the original text to highlight fragments.
                 * Without the term vectors enabled, you will never get highlight results.  I made sure that dynamic indexes
                 * using highlighting will throw if this is not set, but did not want to touch static indexes.
                 *
                 * For MoreLikeThis, you should be using TermVector.Yes since all you need are the term frequencies per document
                 * to know which ones relate.  MoreLikeThis will fall back to reading the terms itself if you set Storage.Yes on
                 * the index field, but it is generally more efficient to use term vectors.  Since MoreLikeThis has been in RavenDB
                 * for awhile and presumably was only working if you stored the field, I would recommend to anyone using MoreLikeThis
                 * to turn on TermVector.Yes for the relevant index field, and turn off Storage.Yes unless you have another reason to
                 * store the field.
                 */
                Indexes.Add(x => x.Text, FieldIndexing.Analyzed);
                IndexSuggestions.Add(x => x.Text, new SuggestionOptions { Accuracy = 0.5f, Distance = StringDistanceTypes.Levenshtein });
                TermVectors.Add(x => x.Text, FieldTermVector.WithPositionsAndOffsets);
                Store(x => x.Text, FieldStorage.Yes);
            }
        }
    }
}