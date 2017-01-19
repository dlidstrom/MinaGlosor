using System;

namespace MinaGlosor.Tool.Dto
{
    public class WordDto
    {
        public string RequestUsername { get; set; }

        public string RequestPassword { get; set; }

        public DateTime CreatedDate { get; set; }

        public string Text { get; set; }

        public string OwnerEmail { get; set; }

        public string Definition { get; set; }

        public string WordListName { get; set; }

        public override string ToString()
        {
            var s = string.Format(
                "CreatedDate: {0}, Text: {1}, Definition: {2}, WordListName: {3}, OwnerEmail: {4}",
                CreatedDate,
                Text,
                Definition,
                WordListName,
                OwnerEmail);
            return s;
        }
    }
}