using Flurl.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PharmacyService.Extensions;
using PharmacyService.Models;
using PharmacyService.Services;
using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PharmacyService.Api.V1
{
    [Route("api/v1/[controller]")]
    public class PharmacyController : ControllerBase
    {
        private readonly StartupOptions _options;
        private readonly ILocationService _locationService;

        public PharmacyController(
            IOptions<StartupOptions> options,
            ILocationService locationService)
        {
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
            _locationService = locationService ?? throw new ArgumentNullException(nameof(locationService));
        }

        [HttpGet("cannary")]
        public IActionResult Cannary() => Ok();

        [HttpGet("list")]
        public async Task<ActionResult<PagedSearch<Pharmacy>>> Get(
            CancellationToken cancellationToken,
            int distance,
            string address,
            int page = 1,
            int pageSize = 20)
        {
            // Get location
            var decodedAddress = Encoding.UTF8.GetString(Convert.FromBase64String(address));
            (double Latitude, double Longitude) = await _locationService.GetLocationAsync(decodedAddress, cancellationToken);
            
            // Get hospitales with this codes
            var returned = await $"{_options.PharmacySource}/api/{_options.Token}/apotheken.json?&search[radius]={distance}&search[offset]={0}&search[sort]={1}&search[location][geographicalPoint][latitude]={Latitude}&search[location][geographicalPoint][longitude]={Longitude}"
                .GetJsonAsync<AppotekenDE>(cancellationToken);

            var pharmacies = returned?.Response?.Pharmacies?.Select(x => x.Value);
            return pharmacies?.Page(page, pageSize);
        }

        [HttpGet("list-emergency")]
        public async Task<ActionResult<PagedSearch<Pharmacy>>> GetEmergency(
            CancellationToken cancellationToken,
            int distance,
            string address,
            int page = 1,
            int pageSize = 20)
        {
            // Get location
            var decodedAddress = Encoding.UTF8.GetString(Convert.FromBase64String(address));
            (double Latitude, double Longitude) = await _locationService.GetLocationAsync(decodedAddress, cancellationToken);

            var now = DateTime.Now;

            // Get hospitales with this codes
            var returned = await $"{_options.PharmacySource}/api/{_options.Token}/notdienst.json?&search[radius]={distance}&search[offset]={0}&search[sort]={1}&search[location][geographicalPoint][latitude]={Latitude}&search[location][geographicalPoint][longitude]={Longitude}&search[startDateTime]={now.Date.ToString("yyyy-MM-dd HH:mm:ss")}&search[endDateTime]={now.Date.AddDays(1).AddSeconds(-1).ToString("yyyy-MM-dd HH:mm:ss")}"
                .GetJsonAsync<AppotekenDE>(cancellationToken);

            var pharmacies = returned?.Response?.Pharmacies?.Select(x => x.Value);
            return pharmacies?.Page(page, pageSize);
        }
    }
}
