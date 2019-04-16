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
    public class ArticleServiceByApothekenDE : IArticleService
    {
        private readonly StartupOptions _options;

        public ArticleServiceByApothekenDE(
            IOptions<StartupOptions> options)
        {
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }

        public async Task<IEnumerable<Article>> GetArticlesForCategoryAsync(int categoryId, CancellationToken cancellationToken)
        {
            var returned = await $"{_options.ArticleSource}/api/{_options.ArticleServiceToken}/artikel.json?&search[categories]={categoryId}&search[limit]={1000}&search[offset]={0}&search[sort]={1}"
                .GetJsonAsync<AppotekenDEArticle>(cancellationToken);

            return returned?.Response?.HealthTodayArticle?.Select(x => x.Value);
        }
    }
}
