using MinaGlosor.Web.Infrastructure;

namespace MinaGlosor.Web.Models.DomainEvents.Handlers
{
    public class ResetPasswordHandler : AbstractHandle<ResetPasswordRequestedEvent>
    {
        public override void Handle(ResetPasswordRequestedEvent ev)
        {
            Emails.ResetPassword(ev.Email, ev.ActivationCode);
        }
    }
}