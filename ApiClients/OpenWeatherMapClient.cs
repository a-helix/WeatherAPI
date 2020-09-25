using Credentials;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ApiClients
{
	 public class OpenWeatherMapClient
	{
		string apiPath;
		string apiKey;
		HttpClient client;

		public OpenWeatherMapClient()
		{
			client = new HttpClient();
			JsonFileContent config = new JsonFileContent("ApiClientKey.json");
			apiPath = "https://us1.locationiq.com/v1/search.php?";
			apiKey = (string)config.selectedParameter("OpenWeatherMap");
		}
	}
}
