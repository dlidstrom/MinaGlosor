using System.Net.Http;
using System.Threading.Tasks;

namespace MinaGlosor.Test.Api.Infrastructure
{
    public static class WordExtensions
    {
        public static async Task<PostWordResponse> PostWord(this WebApiIntegrationTest test, string text, string definition, string wordListId)
        {
            var request = new
            {
                wordListId,
                text,
                definition
            };
            var response = await test.Client.PostAsJsonAsync("http://temp.uri/api/word", request);
            response.EnsureSuccessStatusCode();
            test.WaitForIndexing();
            var content = await response.Content.ReadAsAsync<PostWordResponse>();
            return content;
        }

        public static async Task UpdateWord(this WebApiIntegrationTest test, string wordId, string text, string definition)
        {
            var request = new
            {
                wordId,
                text,
                definition
            };
            var response = await test.Client.PutAsJsonAsync("http://temp.uri/api/word", request);
            response.EnsureSuccessStatusCode();
            test.WaitForIndexing();
        }

        public class PostWordResponse
        {
            public string WordId { get; set; }
        }
    }
}