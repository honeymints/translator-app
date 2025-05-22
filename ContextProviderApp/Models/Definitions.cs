namespace ContextProviderApp.Models;

public class Definition
{
    public string PartOfSpeechContext { get; set; }
    public List<DefinitionBody> Data { get; set; } = new List<DefinitionBody>();

}

public class DefinitionBody
{

    public string Level { get; set; }
    public string Text { get; set; }
    public List<TextBody> Examples { get; set; }
    public List<string> Synonyms { get; set; }

    public List<Phrase> Phrases { get; set; }
}

public class Phrase
{
    public string Title { get; set; }
    public string Text { get; set; }
    public string Example { get; set; }
}

public class TextBody
{
    public string Word { get; set; }
    public string Sentence { get; set; }
}