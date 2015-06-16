namespace MinaGlosor.Tool.Commands
{
    public interface ICommandRunner
    {
        void Run(string username, string password, string[] args);
    }
}