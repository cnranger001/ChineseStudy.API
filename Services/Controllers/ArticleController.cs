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
        public int GetTotal()
        {
            using (var appCon = new AppDbContext())
            {
                var total = appCon.Articles.Count();
                return total;
            }
        }
    }
}
