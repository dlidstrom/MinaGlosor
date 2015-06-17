using System.Collections.Generic;
using System.Linq;
using MinaGlosor.Web.Models.Indexes;
using Raven.Client.Linq;

namespace MinaGlosor.Web.Models.AdminCommands
{
    public class MigrateWordUserIdAdminCommandHandler : AbstractAdminCommandHandler<MigrateWordUserIdAdminCommand>
    {
        public override object Run(MigrateWordUserIdAdminCommand command)
        {
            var migratedWords = new List<string>();
            var current = 0;
            var userIds = new Dictionary<string, string>();
            while (true)
            {
                using (var session = DocumentStore.OpenSession())
                {
                    var query = from word in session.Query<Word, WordIndex>()
                                select word;
                    var words = query.Skip(current).Take(128).ToArray();
                    if (words.Length == 0) break;

                    foreach (var word in words.Where(x => x.UserId == null))
                    {
                        string userId;
                        if (userIds.TryGetValue(word.WordListId, out userId) == false)
                        {
                            var wordList = session.Load<WordList>(word.WordListId);
                            userId = wordList.OwnerId;
                            userIds.Add(wordList.Id, userId);
                        }

                        word.SetUserId(userId);
                        migratedWords.Add(word.Id);
                    }

                    session.SaveChanges();
                    current += words.Length;
                }
            }

            var result = new Result(migratedWords.ToArray());
            return result;
        }

        private class Result
        {
            public Result(string[] migratedWords)
            {
                MigratedWords = migratedWords;
            }

            public string[] MigratedWords { get; private set; }
        }
    }
}