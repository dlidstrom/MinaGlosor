using System;

namespace MinaGlosor.Web.Models.DomainEvents
{
    public class WordExpiredEventHandler : AbstractHandle<WordExpiredEvent>
    {
        public override void Handle(WordExpiredEvent ev)
        {
            throw new NotImplementedException();
        }
    }
}