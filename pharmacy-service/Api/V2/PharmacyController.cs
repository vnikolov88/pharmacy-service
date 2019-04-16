using Microsoft.AspNetCore.Mvc;
using PharmacyService.Extensions;
using PharmacyService.Models;
using PharmacyService.Services;
using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PharmacyService.Api.V2
{
    [Route("api/v2/[controller]")]
    public class PharmacyController : ControllerBase
    {
        private readonly ILocationService _locationService;
        private readonly IPharmacyService _pharmacyService;

        public PharmacyController(
            ILocationService locationService,
            IPharmacyService pharmacyService)
        {
            _locationService = locationService ?? throw new ArgumentNullException(nameof(locationService));
            _pharmacyService = pharmacyService ?? throw new ArgumentNullException(nameof(pharmacyService));
        }

        [HttpGet("cannary")]
        public IActionResult Cannary() => Ok();

        [HttpGet("list")]
        public async Task<ActionResult<PagedSearch<Pharmacy>>> Get(
            CancellationToken cancellationToken,
            int distanceKm,
            string address,
            int page = 1,
            int pageSize = 20)
        {
            var decodedAddress = Encoding.UTF8.GetString(Convert.FromBase64String(address));
            (double latitude, double longitude) = await _locationService.GetLocationAsync(decodedAddress, cancellationToken);
            
            var pharmacies = await _pharmacyService.GetPharmaciesAsync(latitude, longitude, distanceKm, cancellationToken);
            return pharmacies?.Where(x => x.OpeningTimes.Count > 0).Page(page, pageSize);
        }

        [HttpGet("list-emergency")]
        public async Task<ActionResult<PagedSearch<Pharmacy>>> GetEmergency(
            CancellationToken cancellationToken,
            int distanceKm,
            string address,
            int page = 1,
            int pageSize = 20)
        {
            var decodedAddress = Encoding.UTF8.GetString(Convert.FromBase64String(address));
            (double latitude, double longitude) = await _locationService.GetLocationAsync(decodedAddress, cancellationToken);

            var pharmacies = await _pharmacyService.GetEmergencyPharmaciesAsync(latitude, longitude, distanceKm, cancellationToken);
            return pharmacies?.Where(x => x.OpeningTimes.Count > 0).Page(page, pageSize);
        }
    }
}
