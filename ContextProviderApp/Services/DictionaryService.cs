using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using ContextProviderApp.Models;

namespace ContextProviderApp.Services
{

    public class DictionaryService
    {
        public DictionaryService(IHttpClientFactory httpClientFactory)
        {
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
                var POS_HEADER_CHLD = "posgram dpos-g hdib lmr-5";
                var POS_HEADER_CHLD_CHLD = "pos dpos";

                var POS_BD = "pos-body";
                var POS_BD_CHLD1 = "pr dsense";

                var POS_BD_CHLD1_CHLD1 = "dsense_h";

                var POS_BD_CHLD2_CHLD1 = "sense-body dsense_b";
                var POS_BD_CHLD2_CHLD1_CHLD1 = "def-block ddef_block";

                var POS_BD_CHLD2_CHLD1_CHID1_CHLD0 = "hflxrev hdf-xs hdb-s hdf-l";
                var POS_BD_CHLD2_CHLD1_CHID1_CHLD0_CHLD1 = "hflx1";

                var POS_BD_CHLD2_CHLD1_CHLD1_CHLD1 = "ddef_h";
                var POS_BD_CHLD2_CHLD1_CHLD1_CHLD1_CHLD1 = "def-info ddef-info";

                var POS_BD_CHLD2_CHLD1_CHLD1_CHLD1_CHLD2 = "def ddef_d db";

                var POS_BD_CHLD2_CHLD1_CHLD2 = "pr phrase-block dphrase-block";
                var POS_BD_CHLD2_CHLD1_CHLD2_CHLD1 = "phrase-head dphrase_h";
                var POS_BD_CHLD2_CHLD1_CHLD2_CHLD1_CHLD1 = "phrase-title dphrase-title";
                var POS_BD_CHLD2_CHLD1_CHLD2_CHLD1_CHLD2 = "phrase-info dphrase-info";


                var POS_BD_CHLD2_CHLD1_CHLD2_CHLD2 = "phrase-body dphrase_b";

                var POS_BD_CHLD2_CHLD1_CHLD1_CHLD2 = "def-body ddef_b";

                var POS_BD_CHLD2_CHLD1_CHLD1_CHLD2_CHLD1 = "examp dexamp";
                var POS_BD_CHLD2_CHLD1_CHLD1_CHLD2_CHLD1_CHLD1 = "eg deg";
                var POS_BD_CHLD2_CHLD1_CHLD1_CHLD2_CHLD1_CHLD2 = "lu dlu";


                var POS_BD_CHLD2_CHLD1_CHLD1_CHLD2_CHLD2 = "xref synonyms hax dxref-w lmt-25";
                var POS_BD_CHLD2_CHLD1_CHLD1_CHLD2_CHLD2_CHLD1 = "lcs lmt-10 lmb-20";

                var meanings = new List<Meaning> { };


                if (listOfSections != null && listOfSections.Any())
                {
                    foreach (var section in listOfSections)
                    {
                        var contextData = new ContextData();

                        var meaning = new Meaning();

                        var partOfSpeech = section?.Children?.FirstOrDefault(x => x.ClassName.Equals(POS_HEADER))?
                                                    .Children?.FirstOrDefault(x => x.ClassName.Equals(POS_HEADER_CHLD))?
                                                    .Children?.FirstOrDefault(x => x.ClassName.Equals(POS_HEADER_CHLD_CHLD))?.InnerHtml;

                        var contexts = section?.Children?.FirstOrDefault(x => x.ClassName.Contains(POS_BD))?.Children.Where(x => x.ClassName.Contains(POS_BD_CHLD1));

                        if (contexts != null && contexts.Any())
                        {
                            var definitions = new List<Definition> { };
                            foreach (var item in contexts)
                            {
                                var definitionData = new Definition { };
                                var partOfSpeechContextBase = item.Children.FirstOrDefault(x => x.ClassName != null && x.ClassName.Equals(POS_BD_CHLD1_CHLD1));
                                var partOfSpeechContext = string.Empty;

                                if (partOfSpeechContextBase != null)
                                {
                                    foreach (var el in partOfSpeechContextBase?.Children)
                                    {
                                        partOfSpeechContext += " " + el.TextContent.Replace('\n', ' ').Trim();
                                        partOfSpeechContext = partOfSpeechContext.TrimStart();
                                    }
                                }

                                var definitionsInfo = item?.Children?
                                        .FirstOrDefault(x => x.ClassName.Contains(POS_BD_CHLD2_CHLD1))?.Children?
                                        .Where(x => x.ClassName.Contains(POS_BD_CHLD2_CHLD1_CHLD1));

                                foreach (var definitionBlock in definitionsInfo)
                                {
                                    var block = definitionBlock.Children.Any(x => x.ClassName.Contains(POS_BD_CHLD2_CHLD1_CHLD1_CHLD1))
                                                                        ? definitionBlock?.Children?.FirstOrDefault(x => x.ClassName.Contains(POS_BD_CHLD2_CHLD1_CHLD1_CHLD1))?.Children
                                                                        : definitionBlock?.Children?.FirstOrDefault(x => x.ClassName.Contains(POS_BD_CHLD2_CHLD1_CHID1_CHLD0))?.Children?
                                                                                                    .FirstOrDefault(x => x.ClassName.Contains(POS_BD_CHLD2_CHLD1_CHID1_CHLD0_CHLD1))?.Children;

                                    var definitionText = block?.FirstOrDefault(x => x.ClassName.Contains(POS_BD_CHLD2_CHLD1_CHLD1_CHLD1_CHLD2))?.TextContent ??
                                        block?.FirstOrDefault(x => x.ClassName.Contains(POS_BD_CHLD2_CHLD1_CHLD1_CHLD1))?.Children?
                                                         .FirstOrDefault(x => x.ClassName.Contains(POS_BD_CHLD2_CHLD1_CHLD1_CHLD1_CHLD2))?.TextContent.Replace("\n", "").Trim();

                                    var level = block?.FirstOrDefault(x => x.ClassName.Contains(POS_BD_CHLD2_CHLD1_CHLD1_CHLD1_CHLD1))?.TextContent.Split(' ')[0];

                                    var definitionBody = definitionBlock?.Children?.FirstOrDefault(x => x.ClassName.Contains(POS_BD_CHLD2_CHLD1_CHLD1_CHLD2)) ??
                                                        definitionBlock?.Children?.FirstOrDefault(x => x.ClassName.Contains(POS_BD_CHLD2_CHLD1_CHID1_CHLD0))?
                                                        .Children?.FirstOrDefault(x => x.ClassName.Contains(POS_BD_CHLD2_CHLD1_CHID1_CHLD0_CHLD1))?
                                                        .Children?.FirstOrDefault(x => x.ClassName.Contains(POS_BD_CHLD2_CHLD1_CHLD1_CHLD2));

                                    var definitionExamples = definitionBody?.Children.Where(x => x.ClassName.Contains(POS_BD_CHLD2_CHLD1_CHLD1_CHLD2_CHLD1))?.Select(x =>
                                        new TextBody
                                        {
                                            Sentence = x?.Children.FirstOrDefault(x => x.ClassName.Contains(POS_BD_CHLD2_CHLD1_CHLD1_CHLD2_CHLD1_CHLD1))?.TextContent.Trim() ?? string.Empty,
                                            Word = x?.Children.FirstOrDefault(x => x.ClassName.Contains(POS_BD_CHLD2_CHLD1_CHLD1_CHLD2_CHLD1_CHLD2))?.TextContent.Trim() ?? string.Empty
                                        }
                                    ).ToList();

                                    var synonyms = definitionBody?.Children?
                                                                    .FirstOrDefault(x => x.ClassName.Contains(POS_BD_CHLD2_CHLD1_CHLD1_CHLD2_CHLD2))?.Children
                                                                    .FirstOrDefault(x => x.ClassName.Contains(POS_BD_CHLD2_CHLD1_CHLD1_CHLD2_CHLD2_CHLD1))?.Children
                                                                    .Select(x => x.TextContent.Trim())
                                                                    .ToList();

                                    var isPhrasesExists = item?.Children?
                                                                .Any(x => x != null && !string.IsNullOrEmpty(x.ClassName)
                                                                        && x.ClassName.Contains(POS_BD_CHLD2_CHLD1)
                                                                        && x.Children.Any(x => x.ClassName.Contains(POS_BD_CHLD2_CHLD1_CHLD1)));

                                    var phrases = isPhrasesExists.HasValue && isPhrasesExists.Value ? item?.Children?
                                            .FirstOrDefault(x => x.ClassName.Contains(POS_BD_CHLD2_CHLD1))?.Children?
                                            .Where(x => x.ClassName.Contains(POS_BD_CHLD2_CHLD1_CHLD2))?
                                            .Select(x => new Phrase
                                            {
                                                Title = (x.Children?.FirstOrDefault(x => x.ClassName.Contains(POS_BD_CHLD2_CHLD1_CHLD2_CHLD1))?
                                                         .Children?.FirstOrDefault(x => x.ClassName.Contains("title"))?.TextContent),

                                                Text = x.Children?.FirstOrDefault(x => x.ClassName.Contains(POS_BD_CHLD2_CHLD1_CHLD2_CHLD2))?.Children
                                                                .FirstOrDefault(x => x.ClassName.Contains(POS_BD_CHLD2_CHLD1_CHLD1))?.Children?
                                                                .FirstOrDefault(x => x.ClassName.Contains(POS_BD_CHLD2_CHLD1_CHLD1_CHLD1))?.TextContent.Trim(),

                                                Example = x.Children?.FirstOrDefault(x => x.ClassName.Contains(POS_BD_CHLD2_CHLD1_CHLD2_CHLD2))?.Children
                                                                    .FirstOrDefault(x => x.ClassName.Contains(POS_BD_CHLD2_CHLD1_CHLD1))?.Children?
                                                                    .FirstOrDefault(x => x.ClassName.Contains(POS_BD_CHLD2_CHLD1_CHLD1_CHLD2))?.Children?
                                                                    .FirstOrDefault(x => x.ClassName.Contains(POS_BD_CHLD2_CHLD1_CHLD1_CHLD2_CHLD1))?.TextContent

                                            }).ToList()
                                            : new List<Phrase>();

                                    var definitionBodyData = new DefinitionData
                                    {
                                        Level = level,
                                        Text = definitionText,
                                        Examples = definitionExamples,
                                        Synonyms = synonyms,
                                        Phrases = phrases
                                    };
                                    definitionData.Data.Add(definitionBodyData);
                                }
                                definitionData.PartOfSpeechContext = partOfSpeechContext;
                                definitions.Add(definitionData);
                            }
                            meaning.Definitions.AddRange(definitions);

                        }
                        contextData.PartOfSpeech = partOfSpeech;
                        contextData.ContextBody.Add(meaning);
                        result.Contexts.Add(contextData);
                    }
                    result.SourceWord = text;
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
}
