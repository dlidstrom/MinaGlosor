using System.Net.Http;
using System.Threading.Tasks;

namespace MinaGlosor.Test.Api.Infrastructure
{
    public static class WordListExtensions
    {
        public static async Task<Response> PostWordList(this WebApiIntegrationTest test, string name = "some name")
        {
            var request = new
            {
                name
            };
            var response = await test.Client.PostAsJsonAsync("http://temp.uri/api/wordlist", request);
            response.EnsureSuccessStatusCode();
            test.WaitForIndexing();
            var content = await response.Content.ReadAsAsync<Response>();
            return content;
        }

        public class Response
        {
            public string WordListId { get; set; }
        }
    }
}