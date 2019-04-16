using System.Collections.Generic;

namespace PharmacyService.Models
{
    #region Pharmacies
    class ApotekenDEPharmacyResponse
    {
        public IDictionary<string, Pharmacy> Pharmacies { get; set; }
    }

    class AppotekenDEPharmacy
    {
        public ApotekenDEPharmacyResponse Response { get; set; }
    }
    #endregion Pharmacies

    #region Articles
    class ApotekenDEArticleResponse
    {
        public IDictionary<string, Article> HealthTodayArticle { get; set; }
    }

    class AppotekenDEArticle
    {
        public ApotekenDEArticleResponse Response { get; set; }
    }
    #endregion Articles
}
