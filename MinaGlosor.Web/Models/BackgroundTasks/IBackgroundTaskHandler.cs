namespace MinaGlosor.Web.Models.BackgroundTasks
{
    public interface IBackgroundTaskHandler<in TTask>
    {
        void Handle(TTask task);
    }
}