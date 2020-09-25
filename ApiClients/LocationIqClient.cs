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
			apiPath = "api.openweathermap.org/data/2.5/weather?";
			apiKey = (string) config.selectedParameter("LocationIq");
		}

		public async Task<string> apiRequest(string input)
		{
			string location = input.Trim().Replace(" ", "%20");
			string request = string.Format("{0}key={1}&format=json&{2}", apiPath, apiKey, location);
			string response = await client.GetStringAsync(request);
			var responseJson = new JsonStringContent(response);
			string result = String.Join((string) responseJson.selectedParameter("lat"),
										(string) responseJson.selectedParameter("lon"),
										";");
			return result;
		}
	}
}

