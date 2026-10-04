using System.Text;
using System.Text.Json;
using GoogleSearchApp.Models;

namespace GoogleSearchApp.Services
{
    public class SearchService
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public SearchService(IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClient = new HttpClient();
        }

        public async Task<List<SearchResult>> SearchAsync(string query)
        {
            var apiKey = _configuration["Serper:ApiKey"];

            var request = new HttpRequestMessage(
                HttpMethod.Post,
                "https://google.serper.dev/search");

            request.Headers.Add("X-API-KEY", apiKey);

            var requestBody = new
            {
                q = query
            };

            var json = JsonSerializer.Serialize(requestBody);

            request.Content = new StringContent(
                json,
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();

            var responseJson = await response.Content.ReadAsStringAsync();

            return ParseResults(responseJson);
        }

        public List<SearchResult> ParseResults(string json)
        {
            using var document = JsonDocument.Parse(json);

            var results = new List<SearchResult>();

            if (document.RootElement.TryGetProperty("organic", out var organicResults))
            {
                foreach (var item in organicResults.EnumerateArray())
                {
                    results.Add(new SearchResult
                    {
                        Title = item.GetProperty("title").GetString() ?? "",
                        Link = item.GetProperty("link").GetString() ?? "",
                        Description = item.TryGetProperty("snippet", out var snippet)
                            ? snippet.GetString() ?? ""
                            : ""
                    });
                }
            }

            return results;
        }
    }
}