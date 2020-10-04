using System;
using System.Collections.Generic;
using System.Text;

namespace ApiClients
{
    public interface IApiClient
    {
        public ApiResponse apiRequest(string place);
    }
}
