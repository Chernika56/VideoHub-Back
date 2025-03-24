namespace BackEnd.ServerServices
{
    public interface IStreamerService
    {
        Task<HttpResponseMessage> GetStreamsAsync();

        Task<HttpResponseMessage> GetStreamAsync(string name);

        Task<HttpResponseMessage> UpdateStreamAsync(string name, object data);

        Task<HttpResponseMessage> DeleteStreamAsync(string name);
    }
}
