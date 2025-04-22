namespace TranslatorApp.RequestModels
{
    public record ReversoResponse(
        string Text,
        string FromLanguage,
        string ToLanguage,
        string Context,
        string Definition,
        string Synonyms,
        string Example
    );
}