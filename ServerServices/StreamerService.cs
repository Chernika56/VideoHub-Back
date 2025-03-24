using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace BackEnd.ServerServices
{
    public class StreamerService(HttpClient httpClient) : IStreamerService
    {
        public async Task<HttpResponseMessage> GetStreamsAsync()
        {
            string url = $"http://172.16.0.48/streamer/api/v3/streams";

            HttpResponseMessage response = await httpClient.GetAsync(url);

            return response;
        }

        public async Task<HttpResponseMessage> GetStreamAsync(string name)
        {
            string url = $"http://172.16.0.48/streamer/api/v3/streams/{name}";

            HttpResponseMessage response = await httpClient.GetAsync(url);

            return response;
        }

        public async Task<HttpResponseMessage> UpdateStreamAsync(string name, object data)
        {
            string url = $"http://172.16.0.48/streamer/api/v3/streams/{name}";

            string json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PutAsync(url, content);

            return response;
        }

        public async Task<HttpResponseMessage> DeleteStreamAsync(string name)
        {
            string url = $"http://172.16.0.48/streamer/api/v3/streams/{name}";

            HttpResponseMessage response = await httpClient.DeleteAsync(url);

            return response;
        }
    }
}
