namespace MinaGlosor.Web.Data.Events
{
    public class CreateAcountRequestCreated : IDomainEvent
    {
        public CreateAcountRequestCreated(string email, string activationCode)
        {
            ActivationCode = activationCode;
            Email = email;
        }

        public string Email { get; private set; }

        public string ActivationCode { get; private set; }
    }
}