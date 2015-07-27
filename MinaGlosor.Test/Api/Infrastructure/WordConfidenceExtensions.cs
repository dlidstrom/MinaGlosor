using System.Net.Http;
using System.Threading.Tasks;
using MinaGlosor.Web.Models;

namespace MinaGlosor.Test.Api.Infrastructure
{
    public static class WordConfidenceExtensions
    {
        public static async Task<Response> PostWordConfidence(
            this WebApiIntegrationTest test,
            string practiceSessionId,
            string practiceWordId,
            ConfidenceLevel confidenceLevel)
        {
            var request = new
            {
                practiceSessionId,
                practiceWordId,
                ConfidenceLevel = confidenceLevel
            };
            var response = await test.Client.PostAsJsonAsync("http://temp.uri/api/wordconfidence", request);
            response.EnsureSuccessStatusCode();
            test.WaitForIndexing();
            var content = await response.Content.ReadAsAsync<Response>();
            return content;
        }

        public class Response
        {
            public bool IsFinished { get; set; }
        }
    }
}