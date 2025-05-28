namespace ContextProviderApp.Api.Models;

public class DefinitionData
{
    public string Level { get; set; }
    public string Text { get; set; }
    public List<TextBody> Examples { get; set; }
    public List<string> Synonyms { get; set; }
    public List<Phrase> Phrases { get; set; }
}

public class TextBody
{
    public string Word { get; set; }
    public string Sentence { get; set; }
}
