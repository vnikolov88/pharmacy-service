using Microsoft.AspNetCore.Mvc;
using PharmacyService.Extensions;
using PharmacyService.Models;
using PharmacyService.Services;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PharmacyService.Api.V2
{
    [Route("api/v2/[controller]")]
    public class ArticleController : ControllerBase
    {
        private readonly IArticleService _articleService;

        public ArticleController(IArticleService articleService)
        {
            _articleService = articleService ?? throw new ArgumentNullException(nameof(articleService));
        }

        [HttpGet("cannary")]
        public IActionResult Cannary() => Ok();

        [HttpGet("list")]
        public async Task<ActionResult<PagedSearch<Article>>> Get(
            CancellationToken cancellationToken,
            int categoryId,
            int page = 1,
            int pageSize = 20)
        {
            var articles = await _articleService.GetArticlesForCategoryAsync(categoryId, cancellationToken);
            return articles?.Page(page, pageSize);
        }

        [HttpGet("list-emergency")]
        public async Task<ActionResult<PagedSearch<Article>>> GetEmergency(
            CancellationToken cancellationToken,
            int page = 1,
            int pageSize = 20)
        {
            const int emergencyCategoryId = 213;

            var articles = await _articleService.GetArticlesForCategoryAsync(emergencyCategoryId, cancellationToken);
            return articles?.Page(page, pageSize);
        }
    }
}
