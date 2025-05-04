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

        public async Task<List<Meaning>> GetDictionaryAsync(string text)
        {
            var client = _httpClientFactory.CreateClient("Dictionary");
            HttpResponseMessage response = await client.GetAsync($"api/v2/entries/en/{text}").ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var contentJson = await response.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject<List<Meaning>>(contentJson);
                return result;
            }

            throw new Exception("couldn't fetch data");
        }
    }
}