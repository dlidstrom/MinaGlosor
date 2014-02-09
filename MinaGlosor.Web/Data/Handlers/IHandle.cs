namespace MinaGlosor.Web.Data.Handlers
{
    public interface IHandle<in TEvent>
    {
        void Handle(TEvent @event);
    }
}