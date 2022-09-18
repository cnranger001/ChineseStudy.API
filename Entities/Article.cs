using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Article : Knowledge
    {
        public string Title { get; set; }

        public string? Author { get; set; }

        public string Content { get; set; }
    }
}
