using Credentials;
using RabbitChat;
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
			apiPath = (string)config.selectedParameter("OpenWeatherMapUrl");
			apiKey = (string)config.selectedParameter("OpenWeatherMapKey");
		}

		public async void apiRequest(string input)
		{
			string[] parameters = input.Split(";");
			string latitude = parameters[0];
			string longitue = parameters[1];
			string request = string.Format("{0}lat={1}&lon={2}&appid={3}", apiPath, latitude, longitue, apiKey);
			string response = await client.GetStringAsync(request);
			var result = new JsonStringContent(response).ToString();
			Publisher publisher = new Publisher("localhost", "OpenWeatherMapClient", "OpenWeatherMapClient");
			// specify values
			publisher.send(input, result);
		}
	}
}
