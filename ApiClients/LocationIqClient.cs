using Credentials;
using System;
using System.Net.Http;
using System.Threading.Tasks;


namespace ApiClients
{
	public class LocationIqClient
	{
		string apiPath;
		string apiKey;
		HttpClient client;

		public LocationIqClient()
		{
			client = new HttpClient();
			JsonFileContent config = new JsonFileContent("ApiClientKey.json");
			apiPath = (string) config.selectedParameter("LocationIqUrl");
			apiKey = (string) config.selectedParameter("LocationIqKey");
		}

		public async Task<string> apiRequest(string input)
		{
			string location = input.Trim().Replace(" ", "%20");
			string request = string.Format("{0}key={1}&format=json&{2}", apiPath, apiKey, location);
			string response = await client.GetStringAsync(request);
			var responseJson = new JsonStringContent(response).ToString();
			var content = new JsonStringContent(responseJson);
			string result = String.Join((string) content.selectedParameter("lat"),
										(string) content.selectedParameter("lon"),
										";");
			return result;
		}
	}
}

