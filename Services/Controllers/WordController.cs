﻿using Entities;
using Microsoft.AspNetCore.Mvc;

namespace Services.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WordController : ControllerBase
    {
        [HttpGet]
        [Route("singleword")]
        public Word GetSingleWord(string name)
        {
            using (var appCon = new AppDbContext())
            {
                var word = appCon.Words.FirstOrDefault(x => x.Name == name);

                if (word == null)
                {
                    return new Word() { Name = name };
                }
                else
                {
                    return word;
                }
            }
        }

        [HttpGet]
        [Route("total")]
        public int GetTotal(int userId)
        {
            using (var app = new AppDbContext())
            {
                var total =  app.StudyHistory.Where(x => x.UserId == userId && x.KnowledgeType == KnowledgeType.Word)
                    .Select(x => x.KnowledgeId).Distinct().Count();

                return total;
            }
        }

        [HttpGet]
        [Route("{tag}")]
        public IEnumerable<Word> Get(string tag)
        {
            using (var appCon = new AppDbContext())
            {
                var originalList = appCon.Words.Where(x => x.Tags.Contains("，") ? x.Tags.Contains(tag) : x.Tags == tag);

                return originalList.ToList();
            }
        }


        [HttpGet]
        [Route("alltags")]
        public IEnumerable<string> GetAllTags()
        {
            using (var appCon = new AppDbContext())
            {
                var tags = appCon.Words.Select(x=>x.Tags);

                List<string> result = new List<string>();

                foreach (var item in tags)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        if (item.Contains("，"))
                        { 
                           foreach (var tag in item.Split('，'))
                            {
                                if (!result.Contains(tag))
                                    { result.Add(tag); }                       
                            }
                        }
                        else
                        {
                            if (!result.Contains(item)) 
                            { result.Add(item); }                  
                        }
                    }                   
                }

                return result;
            }
        }


        [HttpGet]
        [Route("relatedwords")]
        public IEnumerable<Word> GetRelatedWords(int id)
        {
            using (var appCon = new AppDbContext())
            {
                var word = appCon.Words.FirstOrDefault(x => x.Id == id);
                var list = new List<Word>();
                if (word != null && !string.IsNullOrEmpty(word.RelatedWords))
                {
                    var relatedWords = word.RelatedWords.Split("，");

                    list = appCon.Words.Where(x => relatedWords.Contains(x.Name)).ToList();
                }

                return list;
            }
        }

        [HttpGet]
        public IEnumerable<Word> Get(int UserId, bool needMoreRepetition, int backDays)
        {
            var helper = new WordHelper();

            return helper.GetByStudyHistory<Word>(UserId, needMoreRepetition, backDays, KnowledgeType.Word);
        }

        //[HttpPost]
        //public void Insert([FromBody] Word word, int userId)
        //{
        //    var wordId = 0;
        //    var isNew = false;
        //    using (var app = new AppDbContext())
        //    {
        //        Word update = app.Words.FirstOrDefault(x => x.Name == word.Name.Trim());

        //        if (update != null)
        //        {
        //            if (!string.IsNullOrEmpty(word.Sentence)) update.Sentence = word.Sentence;
        //            if (!string.IsNullOrEmpty(word.RelatedWords)) update.RelatedWords = word.RelatedWords;
        //            if (!string.IsNullOrEmpty(word.Idioms)) update.Idioms = word.Idioms;
        //            if (!string.IsNullOrEmpty(word.Poems)) update.Poems = word.Poems;
        //            if (!string.IsNullOrEmpty(word.Tags)) update.Tags = word.Tags;
        //            update.LastUpdated = DateTime.Now;
        //        }
        //        else
        //        {
        //            app.Words.Add(word);
        //            isNew = true;
        //        }

        //        app.SaveChanges();

        //        wordId = word.Id;
        //    }

        //    if (isNew)
        //    {
        //        using (var app2 = new AppDbContext())
        //        {
        //            var studyHistory = new StudyHistory();
        //            studyHistory.UserId = userId;
        //            studyHistory.StudyTime = DateTime.Now;
        //            studyHistory.KnowledgeType = KnowledgeType.Word;
        //            studyHistory.KnowledgeId = wordId;
        //            studyHistory.NeedMoreRepetition = true;

        //            app2.StudyHistory.Add(studyHistory);
        //            app2.SaveChanges();
        //        }
        //    }
        //}

        [HttpPut]
        public void Update([FromBody] Word word, int userId)
        {
            var helper = new WordHelper();
            var existing = false;
            var id = 0;

            using (var app = new AppDbContext())
            {
                existing = app.Words.Any(x => x.Name == word.Name);
                id = helper.UpdateOrInsertWord(word);
            }


            if (!existing)
            {
                var studyHistory = new StudyHistory();
                studyHistory.UserId = userId;
                studyHistory.StudyTime = DateTime.Now;
                studyHistory.KnowledgeType = KnowledgeType.Word;
                studyHistory.KnowledgeId = id;
                studyHistory.NeedMoreRepetition = true;


                var shc = new StudyHistoryController();
                shc.Insert(studyHistory);

            }
        }
    }
}
