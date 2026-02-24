using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Lecture2Guide.Models;
using Microsoft.Extensions.Configuration;

namespace Lecture2Guide.Services
{
    public class TranscriptService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _baseUrl;

        public TranscriptService( HttpClient httpClient, IConfiguration configuration )
        {
            _httpClient = httpClient;

            // Store your API key and base URL in appsettings.json for security
            _apiKey = configuration["TranscriptApi:ApiKey"] ?? throw new ArgumentNullException("API key not configured");
            _baseUrl = configuration["TranscriptApi:BaseUrl"] ?? "https://transcriptapi.com/api/v2";
        }

        public async Task<VideoTranscript?> GetTranscriptAsync( string videoUrl )
        {
            if (string.IsNullOrWhiteSpace(videoUrl))
                return null;

            try
            {
                // Build the request URL
                var requestUrl = $"{_baseUrl}/youtube/transcript?video_url={Uri.EscapeDataString(videoUrl)}";

                // Add Bearer token
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

                // Make the GET request
                var response = await _httpClient.GetAsync(requestUrl);

                if (!response.IsSuccessStatusCode)
                {
                    // TODO: Optionally handle 402 / 401 / rate limit errors
                    return null;
                }

                var json = await response.Content.ReadAsStringAsync();

                // Deserialize into your model
                var transcript = JsonSerializer.Deserialize<VideoTranscript>(
                    json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );

                return transcript;
            }
            catch
            {
                // Log exception in production
                return null;
            }
        }
    }
}
