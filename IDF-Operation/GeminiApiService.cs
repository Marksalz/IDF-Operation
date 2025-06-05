using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace IDF_Operation
{
    public class GeminiApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public GeminiApiService(string apiKey)
        {
            _httpClient = new HttpClient();
            _apiKey = apiKey;
        }

        public async Task<string> GenerateContentAsync(string prompt)
        {
            var url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent?key={_apiKey}";
            var requestJson = $@"
            {{
                ""contents"": [
                    {{
                        ""parts"": [
                            {{
                                ""text"": ""{prompt}""
                            }}
                        ]
                    }}
                ]
            }}";

            var content = new StringContent(requestJson, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(url, content);
            var responseString = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"API error: {response.StatusCode} - {responseString}");
            }

            JsonDocument document = JsonDocument.Parse(responseString);

            string resultText = document.RootElement
                .GetProperty("candidates")[0]
                .GetProperty("content")
                .GetProperty("parts")[0]
                .GetProperty("text")
                .GetString() ?? string.Empty;

            string cleanedResponse = resultText
                .Replace("\\n", "\n")
                .Replace("\\\"", "\"")
                .Replace("**", "")
                .Replace("```json", "")
                .Replace("```", "")
                .Trim();

            return cleanedResponse;
        }
    }
}
