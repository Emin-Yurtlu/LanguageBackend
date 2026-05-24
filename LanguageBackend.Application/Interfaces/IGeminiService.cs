namespace LanguageBackend.Application.Interfaces
{
    // bu sınıf bır Gemini'den kelime getir sözleşmesi sözleşmesi dır yanı ınterfacedır nasıl getırılecegını bılmez 
    public interface IGeminiService
    {
        // Kullanıcının seviyesine göre, daha önce görmediği bir kelime döndürür
        // excludedWords → daha önce görülen kelimeler, bunları Gemini'ye "çıkar" diyeceğiz
        Task<(string EnglishWord, string TurkishMeaning, string ExampleSentence, string ExampleSentenceTr)> GetNewWordAsync(string level, List<string> excludedWords);
    }
}
