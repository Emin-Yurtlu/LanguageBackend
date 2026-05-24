namespace LanguageBackend.Application.Features.Word.Queries.GetWord
{
    public class GetWordResult
    {
        public string EnglishWord { get; set; } = string.Empty;
        public string TurkishMeaning { get; set; } = string.Empty;
        public string ExampleSentence { get; set; } = string.Empty;
        public string ExampleSentenceTr { get; set; } = string.Empty;
    }
}
