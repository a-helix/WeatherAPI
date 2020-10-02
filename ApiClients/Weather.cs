namespace ApiClients
{
    public class Weather
    {
        public string latitude;
        public string longitude;
        public string temperature;
        public string humidity;
        public string pressure;
        public string timezone;
        public string weather;

        public Weather(string latitude, string longitude, string temperature, string humidity, string pressure, string timezone, string weather)
        {
            this.latitude = latitude;
            this.longitude = longitude;
            this.temperature = temperature;
            this.humidity = humidity;
            this.pressure = pressure;
            this.timezone = timezone;
            this.weather = weather;
        }
    }
}
