using Entities;
using Microsoft.AspNetCore.Mvc;

namespace Services.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ArticleController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<Article> GetArticles()
        {
            using (var appCon = new AppDbContext())
            {
                var originalList = appCon.Articles;

                return originalList.OrderByDescending(x => x.Created).ToList();
            }
        }


        [HttpGet]
        [Route("total")]
        public int GetTotal(int userId)
        {
            using (var app = new AppDbContext())
            {
                var total = app.StudyHistory.Where(x => x.UserId == userId && x.KnowledgeType == KnowledgeType.Article)
                    .Select(x => x.KnowledgeId).Distinct().Count();

                return total;
            }
        }
    }
}
