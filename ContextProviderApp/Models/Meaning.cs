using ContextProviderApp.Enums;

namespace ContextProviderApp.Models;

public class Meaning
{
    public PartOfSpeech PartOfSpeech { get; }
    public IList<Definitions> Definitions { get; }
    public List<string> Synonyms { get; }
    public List<string> Antonyms { get; }
}



