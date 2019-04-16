using PharmacyService.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PharmacyService.Services
{
    public interface IArticleService
    {
        Task<IEnumerable<Article>> GetArticlesForCategoryAsync(int categoryId, CancellationToken cancellationToken);
    }
}
