﻿using System.Net.Http;
using System.Threading.Tasks;

namespace MinaGlosor.Test.Api.Infrastructure
{
    public static class PracticeSessionExtensions
    {
        public static async Task<PracticeSessionResponse> StartPracticeSession(this WebApiIntegrationTest test, string wordListId)
        {
            var request = new
            {
                wordListId
            };
            var response = await test.Client.PostAsJsonAsync("http://temp.uri/api/practicesession", request);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsAsync<PracticeSessionResponse>();
            return content;
        }

        public static async Task<PracticeWordResponse> GetNextPracticeWord(this WebApiIntegrationTest test, string practiceSessionId)
        {
            var response = await test.Client.GetAsync("http://temp.uri/api/practiceword?practiceSessionId=" + practiceSessionId);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsAsync<PracticeWordResponse>();
            return content;
        }

        public class PracticeSessionResponse
        {
            public string PracticeSessionId { get; set; }
        }

        public class PracticeWordResponse
        {
            public string PracticeWordId { get; set; }
        }
    }
}