using System.Net.Http;
using System.Threading.Tasks;

namespace MinaGlosor.Test.Api.Infrastructure
{
    public static class WordListExtensions
    {
        public static async Task<PostWordListResponse> PostWordList(this WebApiIntegrationTest test, string name = "some name")
        {
            var request = new
            {
                name
            };
            var response = await test.Client.PostAsJsonAsync("http://temp.uri/api/wordlist", request);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsAsync<PostWordListResponse>();
            return content;
        }

        public static async Task PublishWordList(this WebApiIntegrationTest test, string id, bool publish)
        {
            var request = new
            {
                id,
                publish
            };
            var response = await test.Client.PostAsJsonAsync("http://temp.uri/api/publishwordlist", request);
            response.EnsureSuccessStatusCode();
        }

        public class PostWordListResponse
        {
            public string WordListId { get; set; }
        }
    }
}