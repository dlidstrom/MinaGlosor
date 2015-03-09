using MinaGlosor.Web.Infrastructure;

namespace MinaGlosor.Web.Models.DomainEvents
{
    public class CreateAccountRequestCreatedHandler : IHandle<CreateAccountRequestCreated>
    {
        public void Handle(CreateAccountRequestCreated ev)
        {
            Emails.InviteUser(ev.Email, ev.ActivationCode);
        }
    }
}