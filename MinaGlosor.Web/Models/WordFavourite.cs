using System;
using MinaGlosor.Web.Models.DomainEvents;
using Raven.Imports.Newtonsoft.Json;

namespace MinaGlosor.Web.Models
{
    public class WordFavourite : DomainModel
    {
        public WordFavourite(string wordId, string userId)
            : base(GetId(wordId, userId))
        {
            if (wordId == null) throw new ArgumentNullException("wordId");
            if (userId == null) throw new ArgumentNullException("userId");

            Apply(new WordFavouriteRegisteredEvent(Id, wordId, userId, true));
        }

        [JsonConstructor]
        private WordFavourite()
        {
        }

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
            Apply(new ToggleWordFavouriteEvent(Id, !IsFavourite));
        }

        private void ApplyEvent(WordFavouriteRegisteredEvent @event)
        {
            UserId = @event.UserId;
            WordId = @event.WordId;
            IsFavourite = @event.IsFavourite;
        }

        private void ApplyEvent(ToggleWordFavouriteEvent @event)
        {
            IsFavourite = @event.IsFavourite;
        }
    }
}