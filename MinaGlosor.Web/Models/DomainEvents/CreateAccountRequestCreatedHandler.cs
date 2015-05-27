using MinaGlosor.Web.Infrastructure;

namespace MinaGlosor.Web.Models.DomainEvents
{
    public class CreateAccountRequestCreatedHandler : IHandle<CreateAccountRequestCreatedEvent>
    {
        public void Handle(CreateAccountRequestCreatedEvent ev)
        {
            Emails.InviteUser(ev.Email, ev.ActivationCode);
        }
    }
}