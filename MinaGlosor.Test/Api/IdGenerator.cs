namespace MinaGlosor.Test.Api
{
    public class IdGenerator
    {
        private int id;

        public int NextId()
        {
            return ++id;
        }
    }
}