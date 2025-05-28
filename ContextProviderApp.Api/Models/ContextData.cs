
namespace ContextProviderApp.Api.Models
{
    public class ContextData
    {
        public string PartOfSpeech { get; set; }
        public List<Meaning> ContextBody { get; set; } = new List<Meaning> { };
    }
}
