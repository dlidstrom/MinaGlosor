using System;
using Raven.Imports.Newtonsoft.Json;

namespace MinaGlosor.Web.Data.Models
{
    public class WordList
    {
        public WordList(string name, User owner)
        {
            if (name == null) throw new ArgumentNullException("name");
            if (owner == null) throw new ArgumentNullException("owner");
            Name = name;
            OwnerId = owner.Id;
        }

        [JsonConstructor]
        private WordList(string name, string ownerId)
        {
            Name = name;
            OwnerId = ownerId;
        }

        public string Id { get; set; }

        public string Name { get; private set; }

        public string OwnerId { get; private set; }

        public WordAnswer Answer(Word word, User user)
        {
            // TODO: Access control
            return new WordAnswer(word, this, user);
        }

        public Word AddWord(string text, string definition)
        {
            return new Word(this, text, definition);
        }
    }
}