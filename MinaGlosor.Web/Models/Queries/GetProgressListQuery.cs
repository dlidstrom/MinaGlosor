using System;
using MinaGlosor.Web.Infrastructure;
using MinaGlosor.Web.Models.Domain.WordListProgressModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MinaGlosor.Web.Models.Queries
{
    public class GetProgressListQuery : IQuery<GetProgressListQuery.Result>
    {
        public GetProgressListQuery(string userId, int page, int itemsPerPage)
        {
            if (userId == null) throw new ArgumentNullException("userId");
            UserId = userId;
            Page = page;
            ItemsPerPage = itemsPerPage;
        }

        public string UserId { get; private set; }

        public int Page { get; private set; }

        public int ItemsPerPage { get; private set; }

        public class Result
        {
            public Result(
                ProgressResult[] progresses,
                int numberOfFavourites,
                int totalItems,
                int currentPage,
                int itemsPerPage)
            {
                Progresses = progresses;
                NumberOfFavourites = numberOfFavourites;
                Paging = new Paging(totalItems, currentPage, itemsPerPage);
            }

            public int NumberOfFavourites { get; private set; }

            public ProgressResult[] Progresses { get; private set; }

            public Paging Paging { get; private set; }
        }

        public class ProgressResult
        {
            public ProgressResult(Progress progress, WordList wordList)
            {
                WordListId = WordList.FromId(progress.WordListId);
                OwnerId = User.FromId(progress.OwnerId);
                Name = wordList.Name;
                NumberOfWords = wordList.NumberOfWords;
                PercentDone = progress.PercentDone;
                PercentExpired = progress.PercentExpired;
                PublishState = wordList.PublishState;
            }

            public string WordListId { get; private set; }

            public string OwnerId { get; private set; }

            public string Name { get; private set; }

            public int NumberOfWords { get; private set; }

            public int PercentDone { get; private set; }

            public int PercentExpired { get; private set; }

            [JsonConverter(typeof(StringEnumConverter))]
            public WordListPublishState PublishState { get; private set; }
        }
    }
}