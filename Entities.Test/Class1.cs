using NUnit.Framework;

namespace Entities.Test
{
    public class Class1
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test()
        {
            var app = new AppDbContext();

            var w = new Word()
            {
                Created = DateTime.Now,
                Name = "鑫",
            };      
            
            app.Words.Add(w);

            app.SaveChanges();
        }
    }
}