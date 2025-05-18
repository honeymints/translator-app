namespace ContextProviderApp.Models;

public class Definitions
{
    public string Level { get; set; }
    public string Definition { get; set; }
    public List<string> Examples { get; set; }
    public List<string> Synonyms { get; set; }

    public List<Phrase> Phrases { get; set; }
}

public class Phrase
{
    public string Text { get; set; }
    public string Example { get; set; }
}