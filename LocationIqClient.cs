using System;
using Credentials;


public class LocatinIqClient
{
	public LocatinIqClient()
	{
		JsonContent config = new JsonContent("ApiClientKey.json");
		string apiPath = "https://us1.locationiq.com/v1/search.php?";
		string apiKey = config;

	}
}
