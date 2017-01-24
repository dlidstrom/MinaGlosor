using JetBrains.Annotations;
using Raven.Imports.Newtonsoft.Json;

namespace MinaGlosor.Web.Models.Domain.ProgressModel
{
    public class ProgressSortOrder
    {
        private ProgressSortOrder(int order)
        {
            Order = order;
            NumberOfWords = 1;
            PercentDone = 0;
            PercentExpired = 101;
        }

        private ProgressSortOrder(int order, int numberOfWords, int percentDone, int percentExpired)
        {
            Order = order;
            NumberOfWords = numberOfWords;
            PercentDone = percentDone;
            PercentExpired = percentExpired;
        }

        [JsonConstructor, UsedImplicitly]
        private ProgressSortOrder()
        {
        }

        public int NumberOfWords { get; private set; }

        public int PercentDone { get; private set; }

        public int PercentExpired { get; private set; }
        
        public int Order { get; private set; }

        public static ProgressSortOrder Default(int order)
        {
            return new ProgressSortOrder(order);
        }

        public static ProgressSortOrder UpdateWith(int order, int numberOfWords, ProgressPercentages progressPercentages)
        {
            return new ProgressSortOrder(
                order,
                numberOfWords > 0 ? -1 : 1,
                progressPercentages.PercentDone < 100 ? 0 : 1,
                progressPercentages.PercentExpired == 0 ? 101 : progressPercentages.PercentExpired);
        }
    }
}