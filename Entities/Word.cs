using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    [Table("Words", Schema = "dbo")]
    public class Word : Knowledge
    {
        public Word()
        {
            Created = DateTime.Now;
        }

        public string Name { get; set; }

        public string? RelatedWords { get; set; }

        public string? Sentence { get; set; }
        
        public string? Idioms { get; set; }
                
        public string? Poems { get; set; } 
        public string? Tags { get; set; }

        public bool? IsQiao { get; set; }

    }

    public class WordComparer : IEqualityComparer<Word>
    {
        public bool Equals(Word? x, Word? y)
        {
            return x.Name == y.Name;
        }

        public int GetHashCode([DisallowNull] Word obj)
        {
            return obj.Name.GetHashCode();
        }
    }
}
