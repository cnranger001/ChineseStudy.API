using Entities;
using Microsoft.AspNetCore.Mvc;

namespace Services.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PoemController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<Poem> GetPoems()
        {
            using (var appCon = new AppDbContext())
            {
                var originalList = appCon.Poems;

                return originalList.OrderByDescending(x=>x.Created).ToList();
            }            
        }

        [HttpGet]
        [Route("total")]
        public int GetTotal()
        {
            using (var appCon = new AppDbContext())
            {
                var total = appCon.Poems.Count();
                return total;
            }
        }
    }
}
