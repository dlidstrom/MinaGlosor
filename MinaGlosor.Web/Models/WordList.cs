using System;
using Raven.Imports.Newtonsoft.Json;

namespace MinaGlosor.Web.Models
{
    public class WordList
    {
        public WordList(string name, User owner)
        {
            if (name == null) throw new ArgumentNullException("name");
            if (owner == null) throw new ArgumentNullException("owner");

            if (name.Length > 1024)
            {
                throw new ArgumentOutOfRangeException("name", "Name cannot be longer than 1024 characters");
            }

            Name = name;
            OwnerId = owner.Id;
        }

        [JsonConstructor]
        private WordList()
        {
        }

        public string Id { get; private set; }

        public string Name { get; private set; }

        public string OwnerId { get; private set; }

        public int NumberOfWords { get; private set; }

        public static string FromId(string wordListId)
        {
            if (wordListId == null) throw new ArgumentNullException("wordListId");
            return wordListId.Substring(10);
        }

        public static string ToId(string wordListId)
        {
            if (wordListId == null) throw new ArgumentNullException("wordListId");
            return "WordLists/" + wordListId;
        }

        public void AddWord()
        {
            NumberOfWords++;
        }

        public bool HasAccess(string userId)
        {
            if (userId == null) throw new ArgumentNullException("userId");

            return OwnerId == userId;
        }
    }
}