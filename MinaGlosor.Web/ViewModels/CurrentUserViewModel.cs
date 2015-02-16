using System;

namespace MinaGlosor.Web.ViewModels
{
    public class CurrentUserViewModel
    {
        public CurrentUserViewModel(bool isAdmin, string username)
        {
            if (username == null) throw new ArgumentNullException("username");
            IsAdmin = isAdmin;
            Username = username;
        }

        public bool IsAdmin { get; private set; }

        public string Username { get; private set; }
    }
}