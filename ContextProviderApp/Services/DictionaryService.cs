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

        public async Task<Result> GetDictionaryAsync(string text)
        {
            var client = _httpClientFactory.CreateClient("Dictionary");
            HttpResponseMessage response = client.GetAsync($"api/v2/entries/en/{text}").ConfigureAwait(false).GetAwaiter().GetResult();

            if (response.IsSuccessStatusCode)
            {
                var contentJson = await response.Content.ReadAsStringAsync();

                var trimmedJson = contentJson.TrimStart('[').TrimEnd(']');

                var result = JsonConvert.DeserializeObject<Result>(trimmedJson);
                return result;
            }

            throw new Exception("couldn't fetch data");
        }
    }

    public class Result
    {   
        [JsonProperty("word")]
        public string Word { get; set; }
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