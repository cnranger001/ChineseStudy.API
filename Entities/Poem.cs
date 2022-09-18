using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Poem : Knowledge
    {      
        public string Dynasty { get; set; }
        public string Author { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public string? RelatedPoems { get; set; }

        public string? Background { get; set; }

        public string? Tags { get; set; }

    }
}
