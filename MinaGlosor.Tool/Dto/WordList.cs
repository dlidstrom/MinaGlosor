namespace MinaGlosor.Tool.Dto
{
    public class WordList
    {
        public string RequestUsername { get; set; }

        public string RequestPassword { get; set; }

        public string Name { get; set; }

        public string OwnerEmail { get; set; }

        public override string ToString()
        {
            var s = string.Format("Name: {0}, OwnerEmail: {1}", Name, OwnerEmail);
            return s;
        }
    }
}