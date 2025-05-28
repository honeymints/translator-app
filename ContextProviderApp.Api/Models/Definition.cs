namespace ContextProviderApp.Api.Models;

public class Definition
{
    public string PartOfSpeechContext { get; set; }
    public List<DefinitionData> Data { get; set; } = new List<DefinitionData>();
}
