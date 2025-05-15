using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using ContextProviderApp.Models;
using Newtonsoft.Json;

namespace ContextProviderApp.Services
{

    public class DictionaryService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public DictionaryService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<string?> GetDictionaryAsync(string text)
        {
            try
            {
                AngleSharp.IConfiguration configuration = Configuration.Default.WithDefaultLoader();
                string adress = $"https://dictionary.cambridge.org/us/dictionary/english/{text}";
                IBrowsingContext context = BrowsingContext.New(configuration);
                IDocument doc = await context.OpenAsync(new Url(adress));
                string title = "div.di-title > span";
                string mainbodyEl = "div.pr.dictionary";
                var cell = doc.QuerySelector<IHtmlElement>(title);
                var mainBodyElement = doc.QuerySelectorAll<IHtmlElement>(mainbodyEl).FirstOrDefault(x => x.GetAttribute("data-id").Equals("cald4-us"));
                var mainSectionElement = mainBodyElement?.Children.FirstOrDefault(x => x.ClassName.Equals("link"));
                var listOfSectionNumber = mainSectionElement?.Children?.FirstOrDefault(x => x.ClassName.Equals("pr di superentry"))?.Children?.Count(x => x.ClassName.Equals("di-body"));

                string a = "div.pos-header.dpos-h";
                var headers = doc.QuerySelector<IHtmlElement>(a);
                for (int i = 0; i < listOfSectionNumber; i++)
                {


                }

                var b = cell?.Children.FirstOrDefault()?.InnerHtml;

                return b;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new Exception("couldnt fetch data");
            }
        }
    }

    public class Result
    {
        [JsonProperty("word")]
        public string Word { get; set; }
        [JsonProperty("phonetic")]
        public string Phonetic { get; set; }
        [JsonProperty("meanings")]
        public List<Meaning> Meanings { get; set; }
        [JsonProperty("phonetics")]
        public List<Phonetic> Phonetics { get; set; }
        [JsonProperty("license")]
        public License License { get; set; }
        [JsonProperty("sourceUrls")]
        public List<string> SourceUrls { get; set; }
    }

    public class Phonetic
    {
        [JsonProperty("audio")]
        public string Audio { get; }
        [JsonProperty("sourceUrl")]
        public string SourceUrl { get; }
        [JsonProperty("license")]
        public License License { get; }
        [JsonProperty("text")]
        public string Text { get; }
    }

    public class License
    {
        [JsonProperty("name")]
        public string Name { get; }
        [JsonProperty("url")]
        public string Url { get; }
    }
}