using Flurl.Http;
using Microsoft.Extensions.Options;
using PharmacyService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PharmacyService.Services
{
    public class PharmacyServiceByApothekenDE : IPharmacyService
    {
        private readonly StartupOptions _options;

        public PharmacyServiceByApothekenDE(
            IOptions<StartupOptions> options)
        {
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }

        public async Task<IEnumerable<Pharmacy>> GetPharmaciesAsync(double latitude, double longitude, double distanceKm, CancellationToken cancellationToken)
        {
            var returned = await $"{_options.PharmacySource}/api/{_options.PharmacyServiceToken}/apotheken.json?&search[radius]={distanceKm}&search[offset]={0}&search[sort]={1}&search[location][geographicalPoint][latitude]={latitude}&search[location][geographicalPoint][longitude]={longitude}"
                .GetJsonAsync<AppotekenDEPharmacy>(cancellationToken);

            return returned?.Response?.Pharmacies?.Select(x => x.Value);
        }

        public async Task<IEnumerable<Pharmacy>> GetEmergencyPharmaciesAsync(double latitude, double longitude, double distanceKm, CancellationToken cancellationToken)
        {
            var now = DateTime.Now;

            var startDateTime = now.Date.ToString("yyyy-MM-dd HH:mm:ss");
            var endDateTime = now.Date.AddDays(1).AddSeconds(-1).ToString("yyyy-MM-dd HH:mm:ss");

            var returned = await $"{_options.PharmacySource}/api/{_options.PharmacyServiceToken}/notdienst.json?&search[radius]={distanceKm}&search[offset]={0}&search[sort]={1}&search[location][geographicalPoint][latitude]={latitude}&search[location][geographicalPoint][longitude]={longitude}&search[startDateTime]={startDateTime}&search[endDateTime]={endDateTime}"
                .GetJsonAsync<AppotekenDEPharmacy>(cancellationToken);

            return returned?.Response?.Pharmacies?.Select(x => x.Value)?.Where(x => x.EndDateTime.Date.Date != now.Date.Date);
        }

    }
}
