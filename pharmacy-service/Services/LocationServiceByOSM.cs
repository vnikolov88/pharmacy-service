using Flurl.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PharmacyService.Services
{
    public class LocationServiceByOSM : ILocationService
    {
        private readonly Random _random = new Random();
        private readonly TimeSpan MaxTTL = TimeSpan.FromDays(30);
        private TimeSpan RetryTTL => TimeSpan.FromHours(_random.Next(1, 24));
        private readonly IMemoryCache _cache;
        private readonly StartupOptions _options;

        public LocationServiceByOSM(
            IMemoryCache cache,
            IOptions<StartupOptions> options)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _options = options.Value ?? throw new ArgumentNullException(nameof(options));
        }

        public async Task<(double Latitude, double Longitude)> GetLocationAsync(string address, CancellationToken cancellationToken)
        {
            if (_cache.TryGetValue(address, out (double Latitude, double Longitude) result))
                return result;

            var encodedAddress = Flurl.Url.Encode(address);
            var location = await $"{_options.OSMRestUrl}/search?q={encodedAddress}&format=json"
                .GetJsonAsync(cancellationToken);

            var ttl = MaxTTL;
            if (location == null)
            {
                result = (0, 0);
                ttl = RetryTTL;
            }
            else
            {
                result = (double.Parse(location.lat), double.Parse(location.lon));
            }

            _cache.Set(address, result, ttl);

            return result;
        }
    }
}
