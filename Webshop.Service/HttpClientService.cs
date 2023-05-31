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
            try
            {
                var response = await _client.GetAsync(url);

                var stringResponse = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<T>(stringResponse);
            }
            catch (Exception ex)
            {
                throw new HttpServiceException("Error occurred while trying to send a GET request.", ex);
            }
        }

        public async Task<TOut> UpdateAsync<TIn, TOut>(string url, TIn data)
        {
            try
            {
                var json = JsonConvert.SerializeObject(data);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _client.PutAsync(url, content);

                var stringResponse = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<TOut>(stringResponse);
            }
            catch (Exception ex)
            {
                throw new HttpServiceException("Error occurred while trying to send an UPDATE request.", ex);
            }
            
        }


        public async Task<TOut> PostAsync<TIn, TOut>(string url, TIn data)
        {
            try
            {
                var json = JsonConvert.SerializeObject(data);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _client.PostAsync(url, content);

                var stringResponse = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<TOut>(stringResponse);
            }
            catch (Exception ex)
            {
                throw new HttpServiceException("Error occurred while trying to send a POST request.", ex);
            }
        }
    }
}
