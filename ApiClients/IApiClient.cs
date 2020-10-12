using DatabaseClient;

namespace ApiClients
{
    public interface IApiClient
    {
        public ApiResponse apiRequest(string place);
    }
}
