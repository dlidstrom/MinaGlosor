using System.Net.Http;
using System.Threading.Tasks;

namespace MinaGlosor.Test.Api.Infrastructure
{
    public static class WordFavouriteExtensions
    {
        public static async Task<Response> MarkWordAsFavourite(this WebApiIntegrationTest test, string wordId, bool isFavourite)
        {
            var request = new
            {
                wordId,
                isFavourite
            };
            var response = await test.Client.PostAsJsonAsync("http://temp.uri/api/wordfavourite", request);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsAsync<Response>();
            return content;
        }

        public class Response
        {
            public string IsFavourite { get; set; }
        }
    }
}