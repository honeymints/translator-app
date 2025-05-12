using ContextProviderApp.Enums;
using Newtonsoft.Json;

namespace ContextProviderApp.Models;

public class Meaning
{
    [JsonProperty("partOfSpeech")]
    public PartOfSpeech PartOfSpeech { get; }
    [JsonProperty("definitions")]
    public IList<Definitions> Definitions { get; }
    [JsonProperty("synonyms")]
    public List<string> Synonyms { get; }
    [JsonProperty("antonyms")]
    public List<string> Antonyms { get; }
}



