using Credentials;
using RabbitChat;
using System;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

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
			JObject json = new JObject(response);
			Weather weather = new Weather(
				(string) json["lat"],
				(string) json["lat"],
				(string) json["current"]["temp"],
				(string) json["current"]["humidity"],
				(string) json["current"]["pressure"],
				(string) json["timezone"],
				(string) json["current"]["weather"]["main"]
				);
			string result = JsonConvert.SerializeObject(weather);
			Publisher publisher = new Publisher("localhost", "OpenWeatherMapClient", "OpenWeatherMapClient");
			publisher.send(input, result);
		}
	}
}
