using Entities;

namespace Services
{
    public class WordHelper
    {
        public IEnumerable<T> GetByStudyHistory<T>(int UserId, bool needMoreRepetition, int backDays, KnowledgeType knowledgeType) where T : Knowledge
        {
            var returnList = new List<T>();
            List<T> originalList = new List<T>();

            using (var appCon = new AppDbContext())
            {
                List<StudyHistory> history;

                if (backDays > 1)
                {
                    history = appCon.StudyHistory.Where(x => x.UserId == UserId && x.KnowledgeType == knowledgeType &&
                  x.StudyTime >= DateTime.Today.AddDays(-backDays) && x.StudyTime < DateTime.Today.AddDays(-backDays + 1)).ToList();
                }
                else
                {
                    history = appCon.StudyHistory.Where(x => x.UserId == UserId && x.KnowledgeType == knowledgeType &&
                  x.StudyTime >= DateTime.Today.AddDays(-backDays) && x.StudyTime < DateTime.Now).ToList();
                }

                if (history.Any())
                {
                    var latestHistory = new List<StudyHistory>();
                    var groups = history.GroupBy(x => x.KnowledgeId);
                    foreach (var group in groups)
                    {
                        StudyHistory sh = group.OrderByDescending(x => x.StudyTime).First();

                        if (sh.NeedMoreRepetition == needMoreRepetition)
                            latestHistory.Add(sh);
                    }

                    if (typeof(T) == typeof(Idiom))
                    {
                        originalList = appCon.Idioms.Where(x => latestHistory.Select(h => h.KnowledgeId).Contains(x.Id)).ToList() as List<T>;
                    }
                    if (typeof(T) == typeof(Word))
                    {
                        originalList = appCon.Words.Where(x => latestHistory.Select(h => h.KnowledgeId).Contains(x.Id)).ToList() as List<T>;
                    }
                    if (typeof(T) == typeof(Poem))
                    {
                        originalList = appCon.Poems.Where(x => latestHistory.Select(h => h.KnowledgeId).Contains(x.Id)).ToList() as List<T>;
                    }
                }
            }

            return originalList.OrderBy(x=>x.Created);
        }


        public int UpdateOrInsertWord(Word word)
        {
            using (var app = new AppDbContext())
            {
                Word update = app.Words.FirstOrDefault(x => x.Name == word.Name || x.Id == word.Id);

                if (update == null) 
                {
                    update = new Word();
                    update.Name = word.Name;
                    app.Words.Add(update);
                }

                if (!string.IsNullOrEmpty(word.Sentence)) update.Sentence = word.Sentence;
                if (!string.IsNullOrEmpty(word.RelatedWords))
                {
                    update.RelatedWords = word.RelatedWords;

                    foreach (var w in word.RelatedWords.Split("，"))
                    {
                        var relatedWord = app.Words.FirstOrDefault(x => x.Name == w);

                        if (relatedWord != null)
                        {
                            if (string.IsNullOrEmpty(relatedWord.RelatedWords))
                            {
                                relatedWord.RelatedWords = update.Name;
                            }
                            else
                            {
                                if (!relatedWord.RelatedWords.Contains(update.Name))
                                {
                                    relatedWord.RelatedWords += "，" + update.Name;
                                }
                            }
                        }
                        else
                        {
                            relatedWord = new Word();
                            relatedWord.Name = w;
                            relatedWord.Sentence = word.Sentence;
                            relatedWord.RelatedWords = update.Name;
                          
                            app.Words.Add(relatedWord);
                        }
                    }
                }

                if (!string.IsNullOrEmpty(word.Idioms))
                {
                    update.Idioms = word.Idioms.Trim();
                    SaveIdioms(word.Idioms.Trim());
                }

                if (!string.IsNullOrEmpty(word.Poems)) update.Poems = word.Poems.Trim();
                if (!string.IsNullOrEmpty(word.Tags)) update.Tags = word.Tags.Trim();
                if (word.IsQiao != null) update.IsQiao = word.IsQiao;
               // if (update.IsQiao == "null") update.IsQiao = null; 

                update.LastUpdated = DateTime.Now;                           
                
                app.SaveChanges();

                return update.Id;
            }
        }

        private void SaveIdioms(string idiom)
        {
            if (string.IsNullOrEmpty(idiom)) return;

            string[] idioms = null;

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
                        if (!app2.Idioms.Any(x => x.Name == cy.Trim()))
                        {
                            var w = new Idiom();
                            w.Name = cy.Trim();
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
                    if (!app2.Idioms.Any(x => x.Name == idiom.Trim()))
                    {
                        var w = new Idiom();
                        w.Name = idiom.Trim();
                        w.Created = DateTime.Now;
                        w.LastUpdated = DateTime.Now;
                        app2.Idioms.Add(w);
                        app2.SaveChanges();
                    }
                }
            }
        }

    }
}
