using MinaGlosor.Web.Data.Events;
using MinaGlosor.Web.Infrastructure;

namespace MinaGlosor.Web.Data.Handlers
{
    public class CreateAcountRequestCreatedHandler : IHandle<CreateAcountRequestCreated>
    {
        public void Handle(CreateAcountRequestCreated @event)
        {
            Emails.InviteUser(@event.Email, @event.ActivationCode);
        }
    }
}