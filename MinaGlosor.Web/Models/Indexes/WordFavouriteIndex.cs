using System.Linq;
using Raven.Client.Indexes;

namespace MinaGlosor.Web.Models.Indexes
{
    public class WordFavouriteIndex : AbstractIndexCreationTask<WordFavourite>
    {
        public WordFavouriteIndex()
        {
            Map = favourites => from favourite in favourites
                                select new
                                    {
                                        favourite.UserId,
                                        favourite.WordId,
                                        favourite.IsFavourite
                                    };
        }
    }
}