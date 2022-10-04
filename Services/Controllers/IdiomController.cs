﻿using Entities;
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
        [Route("relatedidioms")]
        public IEnumerable<Idiom> GetRelatedIdioms(int id)
        {
            using (var appCon = new AppDbContext())
            {
                var idiom = appCon.Idioms.FirstOrDefault(x => x.Id == id);
                var list = new List<Idiom>();
                if (idiom != null && !string.IsNullOrEmpty(idiom.RelatedIdioms))
                {
                    var relatedIdioms = idiom.RelatedIdioms.Split("，");

                    list = appCon.Idioms.Where(x => relatedIdioms.Contains(x.Name)).ToList();
                }

                return list;
            }
        }

        [HttpPost]
        public void UpdateRelatedIdioms(int id, string relatedIdioms)
        {
            using (var appCon = new AppDbContext())
            {
                var rootIdiom = appCon.Idioms.FirstOrDefault(x => x.Id == id);

                if (rootIdiom != null)
                {
                    rootIdiom.RelatedIdioms = relatedIdioms;
                    rootIdiom.LastUpdated = DateTime.Now;
                 
                    foreach (var idiom in relatedIdioms.Split("，"))
                    {
                        var relatedIdiom = appCon.Idioms.FirstOrDefault(x => x.Name == idiom);

                        if (relatedIdiom != null)
                        {
                            if (string.IsNullOrEmpty(relatedIdiom.RelatedIdioms))
                            {
                                relatedIdiom.RelatedIdioms = rootIdiom.Name;
                                relatedIdiom.LastUpdated = DateTime.Now;
                            }
                            else
                            {
                                if (!relatedIdiom.RelatedIdioms.Contains(rootIdiom.Name))
                                {
                                    relatedIdiom.RelatedIdioms += "，" + rootIdiom.Name;
                                    relatedIdiom.LastUpdated = DateTime.Now;
                                }
                            }
                        }
                        else
                        {
                            relatedIdiom = new Idiom();
                            relatedIdiom.Name = idiom;
                            relatedIdiom.RelatedIdioms = rootIdiom.Name;

                            appCon.Idioms.Add(relatedIdiom);
                        }
                    }

                    appCon.SaveChanges();
                }
            }
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
