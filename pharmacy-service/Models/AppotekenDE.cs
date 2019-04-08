using System.Collections.Generic;

namespace PharmacyService.Models
{
    class ApotekenDEResponse
    {
        public IDictionary<string, Pharmacy> Pharmacies { get; set; }
    }

    class AppotekenDE
    {
        public ApotekenDEResponse Response { get; set; }
    }
}
