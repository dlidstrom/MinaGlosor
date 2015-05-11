using System;
using Raven.Imports.Newtonsoft.Json;

namespace MinaGlosor.Web.Models
{
    public class WordFavourite
    {
        public WordFavourite(string wordId, string userId)
        {
            if (wordId == null) throw new ArgumentNullException("wordId");
            if (userId == null) throw new ArgumentNullException("userId");

            Id = GetId(wordId, userId);
            UserId = userId;
            WordId = wordId;
        }

        [JsonConstructor]
        private WordFavourite()
        {
        }

        public string Id { get; private set; }

        public string WordId { get; private set; }

        public string UserId { get; private set; }

        public bool IsFavourite { get; private set; }

        public static string GetId(string wordId, string userId)
        {
            if (wordId == null) throw new ArgumentNullException("wordId");
            if (userId == null) throw new ArgumentNullException("userId");

            var id = string.Format("WordFavourite-{0}-{1}", User.FromId(userId), Word.FromId(wordId));
            return id;
        }

        public void Toggle()
        {
            IsFavourite = !IsFavourite;
        }
    }
}