namespace ApiClients
{
    public class Weather
    {
        private string latitude { get; }
        private string longitude { get; }
        private string temperature { get; }
        private string humidity { get; }
        private string pressure { get; }
        private string timezone { get; }
        private string weather { get; }

        public Weather(string latitude, string longitude, string temperature, string humidity, string pressure, string timezone, string weather)
        {
            this.latitude = latitude;
            this.longitude = longitude;
            this.temperature = temperature;
            this.humidity = humidity;
            this.pressure = pressure;
            this.timezone = timezone;
            this.timezone = weather;
        }
    }
}
