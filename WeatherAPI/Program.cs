using System;
using System.Threading;

namespace WeatherAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ServiceInstance launcer = new ServiceInstance(args);
            CancellationToken cancellationToken = new CancellationToken();
            try
            {
                _ = launcer.StartAsync(cancellationToken);
            }
            catch(OperationCanceledException)
            {
                _ = launcer.StopAsync(cancellationToken);
            }
        }
    }
}
