namespace MinaGlosor.Tool.Commands
{
    public interface ICommand
    {
        void Run(string username, string password, string[] args);
    }
}