using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StackOverflow.Models
{
    [Table("Posts")]
    public class Post
    {
        [Key]
        public int PostId { get; set; }
        public string ContentBody { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual List<Answer> Answers { get; set; }
    }
}
