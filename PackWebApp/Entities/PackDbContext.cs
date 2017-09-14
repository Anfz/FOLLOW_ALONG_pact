using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace PackWebApp.Entities
{
    public class PackDbContext : DbContext
    {
        public PackDbContext(DbContextOptions<PackDbContext> options) : base(options)
        {
            
        }
    }
}
