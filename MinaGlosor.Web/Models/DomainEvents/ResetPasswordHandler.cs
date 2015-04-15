using MinaGlosor.Web.Infrastructure;

namespace MinaGlosor.Web.Models.DomainEvents
{
    public class ResetPasswordHandler : AbstractHandle<ResetPasswordRequested>
    {
        public override void Handle(ResetPasswordRequested ev)
        {
            Emails.ResetPassword(ev.Email, ev.ActivationCode);
        }
    }
}