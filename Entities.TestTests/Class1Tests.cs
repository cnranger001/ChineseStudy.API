using Microsoft.VisualStudio.TestTools.UnitTesting;
using Entities.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Test.Tests
{
    [TestClass()]
    public class Class1Tests
    {
        [TestMethod()]
        public void GetStudyHistory()
        {
            using (var app = new AppDbContext())
            {
                var history = app.StudyHistory.Where(x => x.UserId == 1 && x.KnowledgeType == KnowledgeType.Word).ToList()
                    .OrderBy(x => x.KnowledgeId).ThenByDescending(x=>x.StudyTime)
                    .GroupBy(x => x.KnowledgeId);

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
                    if (item.Count() == 2)
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
            }
        }


        [TestMethod()]
        public void InsertCreateDateIntoHistory()
        {
            using (var app = new AppDbContext())
            {
                var words = app.Words;
                var history = app.StudyHistory.ToList();
                var histories = new List<StudyHistory>();

                foreach (var item in words)
                {
                    if (!history.Any(x => x.KnowledgeType == KnowledgeType.Word && x.KnowledgeId == item.Id))
                    {
                        var h = new StudyHistory()
                        {
                            KnowledgeId = item.Id,
                            KnowledgeType = KnowledgeType.Word,
                            UserId = 1,
                            StudyTime = item.Created,
                            NeedMoreRepetition = true
                        };

                        histories.Add(h);
                    }
                }

                app.StudyHistory.AddRange(histories);
                app.SaveChanges();
            }
        }

        [TestMethod()]
        public void InsertIdiomsIntoSeperateTable()
        {
            using (var app = new AppDbContext())
            {
                var words = app.Words.Where(x => !string.IsNullOrEmpty(x.Idioms));

                foreach (var item in words)
                {
                    string[] idioms = null;
                    string idiom = item.Idioms;

                    if (idiom.Contains(","))
                    {
                        idioms = idiom.Split(",");
                    }

                    if (idiom.Contains("，"))
                    {
                        idioms = idiom.Split("，");
                    }

                    if (idioms != null)
                    {
                        foreach (var cy in idioms)
                        {
                            using (var app2 = new AppDbContext())
                            {
                                if (!app2.Idioms.Any(x => x.Name == cy))
                                {
                                    var w = new Idiom();
                                    w.Name = cy;
                                    w.Created = DateTime.Now;
                                    w.LastUpdated = DateTime.Now;
                                    app2.Idioms.Add(w);
                                    app2.SaveChanges();
                                }
                            }
                        }
                    }
                    else
                    {
                        using (var app2 = new AppDbContext())
                        {
                            if (!app2.Idioms.Any(x => x.Name == idiom))
                            {
                                var w = new Idiom();
                                w.Name = idiom;
                                w.Created = DateTime.Now;
                                w.LastUpdated = DateTime.Now;
                                app2.Idioms.Add(w);
                                app2.SaveChanges();
                            }
                        }
                    }
                }
                app.SaveChanges();
            }
        }
    }
}