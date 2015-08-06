namespace MinaGlosor.Web.Infrastructure
{
    public interface ICommand
    {
    }

    public interface ICommand<out TResult>
    {
    }
}