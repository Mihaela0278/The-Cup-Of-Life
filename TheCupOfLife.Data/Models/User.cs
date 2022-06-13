using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheCupOfLife.Data.Models
{
    public class User : IdentityUser
    {
        // Many to one 
        public ICollection<Post> Posts { get; set; } // list of created posts 
    }
}
