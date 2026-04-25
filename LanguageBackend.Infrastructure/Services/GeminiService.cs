using LanguageBackend.Application.Interfaces;
using Microsoft.Extensions.Configuration;
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

        public GeminiService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["GeminiSettings:ApiKey"]!;
            _model = configuration["GeminiSettings:Model"]!;
        }

        public async Task<(string EnglishWord, string TurkishMeaning)> GetNewWordAsync(string level, List<string> excludedWords)
        {
            // Gemini'ye göndereceğimiz prompt
            var excludedList = excludedWords.Count > 0
                ? $"Bu kelimeleri KULLANMA: {string.Join(", ", excludedWords)}."
                : "";

            var prompt = $@"
                Sen bir İngilizce öğretmenisin.
                {level} seviyesine uygun, günlük hayatta kullanılan tek bir İngilizce kelime ver.
                {excludedList}
                Sadece JSON formatında yanıt ver, başka hiçbir şey yazma:
                {{""english"": ""kelime"", ""turkish"": ""türkçe anlamı""}}";

            // Gemini API istek gövdesi
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

            // Gemini endpoint
            var url = $"https://generativelanguage.googleapis.com/v1beta/models/{_model}:generateContent?key={_apiKey}";
            Console.WriteLine($"Gemini URL: {url}");
            var response = await _httpClient.PostAsync(url, content);
            response.EnsureSuccessStatusCode();

            var responseJson = await response.Content.ReadAsStringAsync();

            // Gemini'nin döndürdüğü JSON'dan metni çıkarıyoruz
            using var doc = JsonDocument.Parse(responseJson);
            var text = doc.RootElement
                .GetProperty("candidates")[0]
                .GetProperty("content")
                .GetProperty("parts")[0]
                .GetProperty("text")
                .GetString()!
                .Trim();

            // Gemini bazen ```json ``` ile sarar, temizliyoruz
            text = text.Replace("```json", "").Replace("```", "").Trim();

            // Gelen JSON'ı parse ediyoruz
            using var wordDoc = JsonDocument.Parse(text);
            var english = wordDoc.RootElement.GetProperty("english").GetString()!;
            var turkish = wordDoc.RootElement.GetProperty("turkish").GetString()!;

            return (english, turkish);
        }
    }
}