using System;

namespace MinaGlosor.Web.Models.DomainEvents
{
    public class WordRememberedEvent
    {
        public WordRememberedEvent(string userId, DateTime currentDate)
        {
            if (userId == null) throw new ArgumentNullException("userId");
            UserId = userId;
            CurrentDate = currentDate;
        }

        public string UserId { get; private set; }

        public DateTime CurrentDate { get; private set; }
    }
}