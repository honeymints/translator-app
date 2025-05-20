namespace ContextProviderApp.Models;

public class Meaning
{
    public string PartOfSpeechContext { get; set; }
    public List<Definition> Definitions { get; set; } = new List<Definition>();
}



