using Entities;
using Microsoft.AspNetCore.Mvc;

namespace Services.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IdiomController : ControllerBase
    {
        [HttpGet]
        [Route("all")]
        public IEnumerable<Idiom> GetIdioms()
        {
            using (var appCon = new AppDbContext())
            {
                var originalList = appCon.Idioms;

                return originalList.OrderByDescending(x => x.Created).ToList();
            }
        }

        [HttpGet]
        public IEnumerable<Idiom> Get(int UserId, bool needMoreRepetition, int backDays)
        {
            var helper = new WordHelper();

            return helper.GetByStudyHistory<Idiom>(UserId, needMoreRepetition, backDays, KnowledgeType.Idiom);

        }

        [HttpGet]
        [Route("total")]
        public int GetTotal()
        {
            using (var appCon = new AppDbContext())
            {
                var total = appCon.Idioms.Count();
                return total;
            }
        }
    }
}
