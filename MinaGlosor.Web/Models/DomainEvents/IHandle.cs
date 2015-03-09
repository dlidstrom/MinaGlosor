namespace MinaGlosor.Web.Models.DomainEvents
{
    public interface IHandle<in TEvent>
    {
        void Handle(TEvent ev);
    }
}