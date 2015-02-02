namespace MinaGlosor.Web.ViewModels
{
    public class CurrentUserViewModel
    {
        public CurrentUserViewModel(bool isAdmin)
        {
            IsAdmin = isAdmin;
        }

        public bool IsAdmin { get; private set; }
    }
}