using Entities;
using Microsoft.AspNetCore.Mvc;

namespace Services.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StudyHistoryController : ControllerBase
    // Controller also inherits from ControllerBase. Controller adds View support.
    // So web api controller should inherit from ControllerBase.
    {
        // the path is /StudyHistory  :method get
        [HttpGet]
        public IEnumerable<StudyHistory> Get(int userId)
        {
            var app = new AppDbContext();

            return app.StudyHistory.Where(x => x.UserId == userId).OrderByDescending(x => x.StudyTime).Take(20).ToList();

            //return new string[] { "value1", "value2" };
        }

        //[HttpGet("StudyTime")]
        //public IEnumerable<StudyHistory> GetStudyTime(int userId, DateTime start, DateTime finish)
        //{
        //    var app = new AppDbContext();

        //    return app.StudyHistory.Where(x => x.UserId == userId).OrderByDescending(x => x.StudyTime).Take(20).ToList();

        //    //return new string[] { "value1", "value2" };
        //}


        [HttpGet]
        [Route("ByKnowledge")]
        public IEnumerable<StudyHistory> Get(int userId, KnowledgeType knowledge, int knowledgeId)
        {
            var app = new AppDbContext();

            return app.StudyHistory.Where(x => x.UserId == userId && x.KnowledgeType == knowledge && x.KnowledgeId == knowledgeId).OrderByDescending(x => x.StudyTime).ToList();

            //return new string[] { "value1", "value2" };
        }

        [HttpGet]
        [Route("Curve")]
        public IEnumerable<Word> GetForgetCurve(int userId, KnowledgeType knowledge, int revisionCount)
        {
            using (var app = new AppDbContext())
            {
                var history = app.StudyHistory.Where(x => x.UserId == userId && x.KnowledgeType == knowledge).ToList()
                    .OrderBy(x => x.KnowledgeId).ThenByDescending(x => x.StudyTime)
                    .GroupBy(x => x.KnowledgeId);

                var words = app.Words.ToList();

                var dic = new Dictionary<string, DateTime>();

                var toDelete = new List<int>();

                // 0 study
                // 1 revision: 1 hour
                // 2 revision: 12 hours
                // 3 revision: 24 hours
                // 4 revision: 36 hours
                // 5 revision: 2 days
                // 6 revision: 4 days
                // 7 revision: 7 days
                // 8 revision: 14 days
                // 9 revision: 30 days

                foreach (var item in history)
                {
                    if (item.Count() == revisionCount)
                    {
                        var first = item.First();

                        // if (first.NeedMoreRepetition)
                        {
                            var word = app.Words.FirstOrDefault(x => x.Id == item.Key);

                            if (word != null)
                                dic.Add(word.Name, item.First().StudyTime);
                            else
                            {
                                dic.Add(item.Key.ToString(), first.StudyTime);
                            }
                        }
                    }
                }

                var newDic = dic.OrderByDescending(x => x.Value);
                var wordList = newDic.Select(x=>x.Key).Take(50);

                return words.Where(x => wordList.Contains(x.Name));

            }
        }


        // the path is /StudyHistory  :method post 
        [HttpPost]
        public void Insert([FromBody] StudyHistory studyHistory)
        {
            using (var app = new AppDbContext())
            {
                var sh = app.StudyHistory.FirstOrDefault(x => x.UserId == studyHistory.UserId
                && x.KnowledgeType == studyHistory.KnowledgeType
                && x.KnowledgeId == studyHistory.KnowledgeId
                && x.StudyTime >= DateTime.Now.AddMinutes(-30)); //only keep one record every 30 minutes. 

                if (sh == null)
                {
                    studyHistory.StudyTime = DateTime.Now;
                    app.StudyHistory.Add(studyHistory);
                }
                else
                {
                    sh.StudyTime = DateTime.Now;
                    sh.NeedMoreRepetition = studyHistory.NeedMoreRepetition;
                }

                app.SaveChanges();
            }
        }

        [HttpPut]
        public void InsertByWordName([FromBody] InsertByWordNameDTO sh)
        {
            var helper = new WordHelper();
            var word = (Word)sh;

            var studyHistory = new StudyHistory();
            studyHistory.UserId = sh.UserId;
            studyHistory.StudyTime = DateTime.Now;
            studyHistory.KnowledgeType = KnowledgeType.Word;
            studyHistory.NeedMoreRepetition = sh.NeedMoreRepetition;

            if (word.Id == 0)
            {
                var id = helper.UpdateOrInsertWord(word);
                studyHistory.KnowledgeId = id;
            }
            else
            {
                studyHistory.KnowledgeId = word.Id;
            }

            Insert(studyHistory);
        }

        public class InsertByWordNameDTO : Word
        {
            public int UserId { get; set; }
          
            public bool NeedMoreRepetition { get; set; }
        }
    }
}
