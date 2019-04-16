using PharmacyService.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PharmacyService.Services
{
    public interface IPharmacyService
    {
        Task<IEnumerable<Pharmacy>> GetPharmaciesAsync(double latitude, double longitude, double distanceKm, CancellationToken cancellationToken);
        Task<IEnumerable<Pharmacy>> GetEmergencyPharmaciesAsync(double latitude, double longitude, double distanceKm, CancellationToken cancellationToken);
    }
}
