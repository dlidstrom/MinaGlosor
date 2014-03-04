using System;

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
        }

        private WordList()
        {
        }

        public int Id { get; set; }

        public string Name { get; private set; }

        public int OwnerId { get; private set; }

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