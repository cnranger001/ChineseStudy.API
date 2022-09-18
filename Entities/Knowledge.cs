using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    public class Knowledge
    {
        [Key]
        public int Id { get; set; }

        public DateTime Created { get; set; }

        public DateTime LastUpdated { get; set; }

    }
}
