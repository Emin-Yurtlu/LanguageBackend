using LanguageBackend.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace LanguageBackend.Infrastructure.Services
{
    public class GeminiService : IGeminiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _model;
        private readonly ILogger<GeminiService> _logger;

        public GeminiService(HttpClient httpClient, IConfiguration configuration, ILogger<GeminiService> logger)
        {
            _httpClient = httpClient;
            _apiKey = configuration["GeminiSettings:ApiKey"]!;
            _model = configuration["GeminiSettings:Model"]!;
            _logger = logger;
        }

        public async Task<(string EnglishWord, string TurkishMeaning)> GetNewWordAsync(string level, List<string> excludedWords)
        {
            int maxRetries = 3;
            int retryCount = 0;
            int delayMs = 1000;

            while (retryCount < maxRetries)
            {
                try
                {
                    return await FetchWordFromGeminiAsync(level);
                }
                catch (Exception ex)
                {
                    retryCount++;

                    if ((ex.Message.Contains("ServiceUnavailable") || ex.Message.Contains("TooManyRequests"))
                        && retryCount < maxRetries)
                    {
                        _logger.LogWarning($"Gemini meşgul, {retryCount}. deneme {delayMs}ms sonra...");
                        await Task.Delay(delayMs);
                        delayMs *= 2;
                    }
                    else
                    {
                        _logger.LogError($"Gemini bağlantı hatası: {ex.Message}");
                        throw;
                    }
                }
            }

            throw new Exception("Gemini API'ye bağlanılamadı, lütfen tekrar deneyin.");
        }

        private async Task<(string EnglishWord, string TurkishMeaning)> FetchWordFromGeminiAsync(string level)
        {
            var prompt = $@"
    Sen bir İngilizce öğretmenisin.
    {level} seviyesine uygun, günlük hayatta kullanılan tek bir İngilizce kelime ver.
    Sadece JSON formatında yanıt ver, başka hiçbir şey yazma:
    {{""english"": ""kelime"", ""turkish"": ""türkçe anlamı""}}";

            var requestBody = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new[]
                        {
                            new { text = prompt }
                        }
                    }
                }
            };

            var json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var url = $"https://generativelanguage.googleapis.com/v1beta/models/{_model.Trim()}:generateContent?key={_apiKey}";

            var response = await _httpClient.PostAsync(url, content);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError($"Gemini API hatası: {response.StatusCode}");
                throw new Exception($"Gemini API hatası: {response.StatusCode} - {errorContent}");
            }

            var responseJson = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(responseJson);
            var root = doc.RootElement;

            if (!root.TryGetProperty("candidates", out var candidates) || candidates.GetArrayLength() == 0)
                throw new Exception("Gemini geçersiz yanıt döndürdü.");

            var firstCandidate = candidates[0];

            if (!firstCandidate.TryGetProperty("content", out var contentProp))
                throw new Exception("Gemini geçersiz yanıt döndürdü.");

            if (!contentProp.TryGetProperty("parts", out var parts) || parts.GetArrayLength() == 0)
                throw new Exception("Gemini geçersiz yanıt döndürdü.");

            if (!parts[0].TryGetProperty("text", out var textElement))
                throw new Exception("Gemini geçersiz yanıt döndürdü.");

            var text = textElement.GetString()!.Trim();
            text = text.Replace("```json", "").Replace("```", "").Trim();

            using var wordDoc = JsonDocument.Parse(text);
            var wordRoot = wordDoc.RootElement;

            if (!wordRoot.TryGetProperty("english", out var englishElement) ||
                !wordRoot.TryGetProperty("turkish", out var turkishElement))
                throw new Exception("Gemini geçersiz JSON formatı döndürdü.");

            var english = englishElement.GetString()!;
            var turkish = turkishElement.GetString()!;

            _logger.LogInformation($"Kelime alındı: {english} = {turkish}");

            return (english, turkish);
        }
    }
}