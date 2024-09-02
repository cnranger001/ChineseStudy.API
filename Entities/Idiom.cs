using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Idiom : Knowledge
    {       
        public string Name { get; set; }
        public string? Sentence { get; set; }
        public string? RelatedIdioms { get; set; }
        public string? Story { set; get; }
        public string? Tags { get; set; }

        public string? ImageUrl { get; set; }
  
    }
}
