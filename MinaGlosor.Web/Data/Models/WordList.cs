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

        public int Id { get; set; }

        public string Name { get; private set; }

        public string OwnerId { get; private set; }
    }
}