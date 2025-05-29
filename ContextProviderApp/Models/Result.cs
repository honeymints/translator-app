namespace ContextProviderApp.Models
{
    public class Result
    {
        public string SourceWord { get; set; }
        public List<ContextData> Contexts { get; set; } = new List<ContextData> { };
    }
}
