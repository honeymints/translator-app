using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using ContextProviderApp.Models;

namespace ContextProviderApp.Services
{

    public class DictionaryService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public DictionaryService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<Result?> GetDictionaryAsync(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new BadHttpRequestException("input can't be empty");
            }
            try
            {
                Result result = new Result();
                result.SourceWord = text;
                AngleSharp.IConfiguration configuration = Configuration.Default.WithDefaultLoader();
                string adress = $"https://dictionary.cambridge.org/us/dictionary/english/{text}";
                IBrowsingContext context = BrowsingContext.New(configuration);
                IDocument doc = await context.OpenAsync(new Url(adress));
                string mainbodyEl = "div.pr.dictionary";
                var mainBodyElement = doc.QuerySelectorAll<IHtmlElement>(mainbodyEl).FirstOrDefault(x => x.GetAttribute("data-id").Equals("cald4-us"));
                var mainSectionElement = mainBodyElement?.Children.FirstOrDefault(x => x.ClassName.Equals("link"));
                var listOfSections = mainSectionElement?.Children?
                    .FirstOrDefault(x => x.ClassName.Equals("pr di superentry"))?.Children?
                    .FirstOrDefault(x => x.ClassName.Equals("di-body"))?.Children?
                    .FirstOrDefault(x => x.ClassName.Equals("entry"))?.Children?
                    .FirstOrDefault(x => x.ClassName.Equals("entry-body"))?.Children?
                    .Where(x => x.ClassName.Equals("pr entry-body__el"));

                var POS_HEADER = "pos-header dpos-h";
                var POS_HEADER_CHILD = "posgram dpos-g hdib lmr-5";
                var POS_HEADER_CHILD_CHILD = "pos dpos";

                if (listOfSections != null && listOfSections.Any())
                {
                    result.Contexts = new List<ContextData> { };
                    foreach (var section in listOfSections)
                    {
                        var partOfSpeech = section?.Children?.FirstOrDefault(x => x.ClassName.Equals(POS_HEADER))?
                                                    .Children?.FirstOrDefault(x => x.ClassName.Equals(POS_HEADER_CHILD))?
                                                    .Children?.FirstOrDefault(x => x.ClassName.Equals(POS_HEADER_CHILD_CHILD))?.InnerHtml;
                        var contextData = new ContextData
                        {
                            PartOfSpeech = partOfSpeech
                        };
                        result.Contexts.Add(contextData);
                    }
                }

                return result;
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
        public string SourceWord { get; set; }

        public List<ContextData> Contexts { get; set; }
    }

    public class ContextData
    {
        public string PartOfSpeech { get; set; }
        public List<Meaning> Meanings { get; set; }
    }
}