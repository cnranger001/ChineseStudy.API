using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public enum KnowledgeType
    {
        Word,
        Idiom,
        Poem,
        Article
    }

    [Table("StudyHistory", Schema = "dbo")]
    public class StudyHistory
    {
        [Key]
        public int Id { get; set; }

        public int UserId { set; get; }

        public KnowledgeType KnowledgeType { get; set; }

        public int KnowledgeId { get; set; }

        public bool NeedMoreRepetition { get; set; }   

        [Column("StudyTime")]
        public DateTime StudyTime { get; set; }
    }
}
