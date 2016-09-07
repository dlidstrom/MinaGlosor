namespace MinaGlosor.Web.Models.Queries
{
    public class Paging
    {
        public Paging(int totalItems, int currentPage, int itemsPerPage)
        {
            TotalItems = totalItems;
            CurrentPage = currentPage;
            ItemsPerPage = itemsPerPage;
            HasPages = TotalItems > ItemsPerPage;
        }

        public int TotalItems { get; private set; }

        public int CurrentPage { get; private set; }

        public int ItemsPerPage { get; private set; }

        public bool HasPages { get; private set; }
    }
}