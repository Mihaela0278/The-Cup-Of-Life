using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheCupOfLife.Data.Models
{
    public class Post
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string ImageUrl { get; set; }
        [Required]
        public string Content { get; set; }

        // One to many relation 
        [Required]
        public string UserId { get; set; }
        public User User { get; set; }
        
        public Guid TagId { get; set; }

        public Tag Tag { get; set; }

    }
}
