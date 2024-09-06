using Entities;
using Microsoft.AspNetCore.Mvc;

namespace Services.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PoemController : ControllerBase
    {
        [HttpGet]
        [Route("all")]
        public IEnumerable<Poem> GetPoems()
        {
            using (var appCon = new AppDbContext())
            {
                var originalList = appCon.Poems;

                return originalList.OrderByDescending(x=>x.Created).ToList();
            }            
        }

        [HttpGet]
        [Route("singlepoem")]
        public Poem GetSinglePoem(string name)
        {
            using (var appCon = new AppDbContext())
            {
                var poem = appCon.Poems.FirstOrDefault(x => x.Name == name);

                if (poem == null)
                {
                    return new Poem() { Name = name };
                }
                else
                {
                    return poem;
                }
            }
        }

        [HttpGet]
        public IEnumerable<Poem> Get(int UserId, bool needMoreRepetition, int backDays)
        {
            var helper = new WordHelper();

            return helper.GetByStudyHistory<Poem>(UserId, needMoreRepetition, backDays, KnowledgeType.Poem);

        }

        [HttpGet]
        [Route("total")]
        public int GetTotal(int userId)
        {
            using (var app = new AppDbContext())
            {
                var total = app.StudyHistory.Where(x => x.UserId == userId && x.KnowledgeType == KnowledgeType.Poem).Select(x => x.KnowledgeId).Distinct().Count();

                return total;
            }
        }
    }
}
