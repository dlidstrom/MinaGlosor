using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MinaGlosor.Web.Data.Models
{
    public class WordList
    {
        public WordList(string name, User owner)
        {
            if (name == null) throw new ArgumentNullException("name");
            if (owner == null) throw new ArgumentNullException("owner");

            if (name.Length > 1024)
            {
                throw new ArgumentException(
                    "Name cannot be longer than 1024 characters",
                    "name");
            }

            Name = name;
            OwnerId = owner.Id;

            Words = new Collection<Word>();
        }

        private WordList()
        {
        }

        public int Id { get; set; }

        public string Name { get; private set; }

        public int OwnerId { get; private set; }

        public virtual ICollection<Word> Words { get; private set; }

        public WordAnswer Answer(Word word, User user)
        {
            // TODO: Access control
            return new WordAnswer(word, this, user);
        }

        public Word AddWord(string text, string definition)
        {
            var word = new Word(this, text, definition);
            Words.Add(word);
            return word;
        }
    }
}