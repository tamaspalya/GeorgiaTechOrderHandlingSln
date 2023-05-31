using Newtonsoft.Json;
using System.Text;

namespace Webshop.Service
{
    public class HttpClientService : IHttpClientService
    {
        private readonly HttpClient _client;

        public HttpClientService(HttpClient client)
        {
            _client = client;
        }

        public async Task<T> GetAsync<T>(string url)
        {
            var response = await _client.GetAsync(url);

            var stringResponse = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(stringResponse);
        }

        public async Task<TOut> UpdateAsync<TIn, TOut>(string url, TIn data)
        {
            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PutAsync(url, content);

            var stringResponse = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<TOut>(stringResponse);
        }


        public Task<HttpResponseMessage> PostAsync(string url, HttpContent content)
        {
            throw new NotImplementedException();
        }
    }
}
