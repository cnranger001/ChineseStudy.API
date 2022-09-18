using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Entities.TestTests
{
    [TestClass()]
    public class ExcelParser
    {
        [TestMethod()]
        public void ImportAllWords()
        {
            Excel.Application xlApp = new Excel.Application();
            Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(@"C:\Users\Frank\Jake Chinese Study Log.xlsx");
            Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];
            Excel.Range xlRange = xlWorksheet.UsedRange;

            var app = new AppDbContext();
            var checkList = new List<string>();

            for (var x = 1; x <= 390; x++)
            {
                var words = GetValue(xlRange, x, 2);

                foreach (var w1 in words.Split("，"))
                {
                    foreach (var w in w1.Split(", "))
                    {
                        var word = new Word();
                        word.Name = w.Trim();

                        var dbl = 0d;
                        var dateValue = GetValue(xlRange, x, 1);
                        var isDouble = double.TryParse(dateValue, out dbl);
                        if (isDouble)
                            word.Created = DateTime.FromOADate(dbl);
                        else
                            word.Created = DateTime.Parse(dateValue);

                        word.Sentence = GetValue(xlRange, x, 3);

                        if (!checkList.Contains(word.Name))
                        {
                            app.Words.Add(word);
                            checkList.Add(w.Trim());
                        }
                    }
                }
            }

            xlWorkbook.Close();

            using (app)
            {
                app.SaveChanges();

            }
        }


        [TestMethod()]
        public void ImportAllPoems()
        {
            Excel.Application xlApp = new Excel.Application();
            Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(@"C:\Users\Frank\Jake Chinese Study Log.xlsx");
            Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[2];
            Excel.Range xlRange = xlWorksheet.UsedRange;

            var app = new AppDbContext();

            for (var x = 2; x <= 33; x++)
            {
                var poem = new Poem();

                poem.Dynasty = GetValue(xlRange, x, 1);
                poem.Author = GetValue(xlRange, x, 2);
                poem.Name = GetValue(xlRange, x, 3);
                poem.Content = GetValue(xlRange, x, 4);
                poem.Background = GetValue(xlRange, x, 5);
                poem.Created = DateTime.Now;

                app.Poems.Add(poem);
            }

            xlWorkbook.Close();

            using (app)
            {
                app.SaveChanges();

            }

        }

        private string GetValue(Excel.Range xlRange, int x, int y)
        {
            var returnVal = "";

            if (xlRange.Cells[x, y] != null && xlRange.Cells[x, y].Value2 != null)
            {
                returnVal = xlRange.Cells[x, y].Value2.ToString();
            }
            else
                returnVal = null;

            return returnVal;
        }
    }
}
