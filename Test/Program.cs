// See https://aka.ms/new-console-template for more information
using Entities;

Console.WriteLine("Hello, World!");


var app = new AppDbContext();

var w = new Word();
w.Name = "钟";
w.Sentence = "钟鑫";
w.Idioms = new List<Idiom>()
{
    new Idiom(){ Name = "千辛万苦" },
    new Idiom(){ Name = "万水千山" }
};

w.Poems = null; 

app.Words.Add(w);

app.SaveChanges();


