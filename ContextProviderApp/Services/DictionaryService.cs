using ContextProviderApp.Models;
using HtmlAgilityPack;
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

        public async Task<string> GetDictionaryAsync(string text)
        {
            var client = _httpClientFactory.CreateClient("Dictionary");
           // HttpResponseMessage response = client.GetAsync($"api/v2/entries/en/{text}").ConfigureAwait(false).GetAwaiter().GetResult();
           // HttpResponseMessage response = await client.GetAsync($"https://dictionary.yandex.net/api/v1/dicservice.json/lookup?key={_apiKey}&lang=en-en&text=time");
            string response = await client.GetStringAsync($"https://dictionary.cambridge.org/us/dictionary/english/flabbergast");
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(response);

            return htmlDocument.DocumentNode.InnerText;
            // if (response.IsSuccessStatusCode)
            // {
            //     var contentJson = await response.Content.ReadAsStringAsync();

            //     var trimmedJson = contentJson.TrimStart('[').TrimEnd(']');

            //     var result = JsonConvert.DeserializeObject<Result>(trimmedJson);
            //     return result;
            // }

            throw new Exception("couldn't fetch data");
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