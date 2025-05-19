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

                var POS_BODY = "pos-body";
                var POS_BODY_CHILD1 = "pr dsense";

                var POS_BODY_CHILD1_CHILD1 = "dsense_h";
                var POS_BODY_CHILD1_CHILD1_CHILD1 = "hw dsense_hw";
                var POS_BODY_CHILD1_CHILD1_CHILD2 = "pos dsense_pos";
                var POS_BODY_CHILD1_CHILD1_CHILD3 = "guideword dsense_gw";

                var POS_BODY_CHILD2_CHILD1 = "sense-body dsense_b";
                var POS_BODY_CHILD2_CHILD1_CHILD1 = "def-block ddef_block";
                var POS_BODY_CHILD2_CHILD1_CHILD1_CHILD1 = "ddef_h";

                var levelDiv = "div.ddef_h";
                var levelSpan = "span.def-info.ddef-info > span.epp-xref.dxref";

                var levels = doc.QuerySelectorAll($"{levelDiv} > {levelSpan}");

                if (listOfSections != null && listOfSections.Any())
                {
                    result.Contexts = new List<ContextData> { };
                    foreach (var section in listOfSections)
                    {
                        var partOfSpeech = section?.Children?.FirstOrDefault(x => x.ClassName.Equals(POS_HEADER))?
                                                    .Children?.FirstOrDefault(x => x.ClassName.Equals(POS_HEADER_CHILD))?
                                                    .Children?.FirstOrDefault(x => x.ClassName.Equals(POS_HEADER_CHILD_CHILD))?.InnerHtml;

                        var contexts = section?.Children?.FirstOrDefault(x => x.ClassName.Contains(POS_BODY))?.Children.Where(x => x.ClassName.Contains(POS_BODY_CHILD1));

                        if (contexts != null && contexts.Any())
                        {
                            var contextBody = new Meaning();

                            foreach (var item in contexts.Select((v, i) => new { index = i, value = v }))
                            {
                                var val = item.value;
                                var index = item.index;
                                var partOfSpeechContextBase = val.Children.FirstOrDefault(x => x.ClassName.Equals(POS_BODY_CHILD1_CHILD1));
                                var partOfSpeechContext = string.Empty;
                                foreach (var el in partOfSpeechContextBase.Children)
                                {
                                    partOfSpeechContext += " " + el.TextContent.Replace('\n', ' ').Trim();
                                    partOfSpeechContext = partOfSpeechContext.TrimStart();

                                }
                                contextBody.Definitions = new List<Definitions>();
                                var a = val.Children.FirstOrDefault(x => x.ClassName.Contains(POS_BODY_CHILD2_CHILD1)).Children.FirstOrDefault(x => x.ClassName.Contains(POS_BODY_CHILD2_CHILD1_CHILD1)).Children?.FirstOrDefault(x => x.ClassName.Contains("def ddef_d db"));


                                contextBody.PartOfSpeechContext = partOfSpeechContext;
                            }
                        }


                        var contextData = new ContextData
                        {
                            PartOfSpeech = partOfSpeech,
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
        public List<Meaning> ContextBody { get; set; }
    }
}