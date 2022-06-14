using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheCupOfLife.Data.Models;

namespace TheCupOfLife.Data
{
    public class TagSeed
    {
        public static void Seed(TheCupOfLifeContext context)
        {
            if (context.Tags.Any())
            {
                return;
            }
            context.Tags.AddRange(
                new Tag
                {
                    Id = new Guid(),
                    Name = "Coffee"
                },
                new Tag
                {
                    Id = new Guid(),
                    Name = "Tea"
                });

            context.SaveChanges();
        }
    }
}
