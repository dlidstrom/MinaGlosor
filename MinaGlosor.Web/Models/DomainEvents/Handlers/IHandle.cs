namespace MinaGlosor.Web.Models.DomainEvents.Handlers
{
    public interface IHandle<in TEvent>
    {
        void Handle(TEvent ev);
    }
}