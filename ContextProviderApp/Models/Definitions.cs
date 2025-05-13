using Newtonsoft.Json;

namespace ContextProviderApp.Models;

public class Definitions
{
    [JsonProperty("definition")]
    public string Definition { get; }
    // [JsonProperty("example")]
    // public string Example { get; }
    [JsonProperty("synonyms")]
    public List<string> Synonyms { get; }
    [JsonProperty("antonyms")]
    public List<string> Antonyms { get; }
}
