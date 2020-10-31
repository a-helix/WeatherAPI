using DatabaseClients;

namespace ApiClients
{
    public interface IApiClient
    {
        public ApiResponse ApiRequest(string place);
    }
}
