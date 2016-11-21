using System;
using MinaGlosor.Web.Infrastructure;
using MinaGlosor.Web.Models.Domain.ProgressModel;

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
            public ProgressResult(Progress progress, WordList wordList, User progressOwner, User wordListOwner)
            {
                ProgressId = Progress.FromId(progress.Id);
                WordListId = WordList.FromId(progress.WordListId);
                ProgressOwnerId = User.FromId(progress.OwnerId);
                WordListOwnerId = User.FromId(wordListOwner.Id);
                WordListOwnerUsername = wordListOwner.Username;
                IsBorrowedWordList = progressOwner.Id != wordListOwner.Id;
                Name = wordList.Name;
                NumberOfWords = wordList.NumberOfWords;
                PercentDone = progress.Percentages.PercentDone;
                NumberOfWordsExpired = progress.WordCounts.NumberOfWordsExpired;
                PercentExpired = progress.Percentages.PercentExpired;
                NumberOfEasyWords = progress.WordCounts.NumberOfEasyWords;
                PercentEasyWords = progress.Percentages.PercentEasyWords;
                NumberOfDifficultWords = progress.WordCounts.NumberOfDifficultWords;
                PercentDifficultWords = progress.Percentages.PercentDifficultWords;
                Published = wordList.PublishState == WordListPublishState.Published;
                GravatarHash = progressOwner.GetGravatarHash();
            }

            public string ProgressId { get; private set; }
            
            public string WordListId { get; private set; }

            public string ProgressOwnerId { get; private set; }

            public string WordListOwnerId { get; private set; }

            public string WordListOwnerUsername { get; private set; }

            public bool IsBorrowedWordList { get; private set; }

            public string Name { get; private set; }

            public int NumberOfWords { get; private set; }

            public int PercentDone { get; private set; }

            public int NumberOfWordsExpired { get; private set; }

            public int PercentExpired { get; private set; }

            public int NumberOfEasyWords { get; private set; }

            public int PercentEasyWords { get; private set; }

            public int NumberOfDifficultWords { get; private set; }

            public int PercentDifficultWords { get; private set; }

            public bool Published { get; private set; }

            public string GravatarHash { get; private set; }
        }
    }
}