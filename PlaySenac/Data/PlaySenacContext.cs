using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PlaySenac.Models;

namespace PlaySenac.Data
{
    public class PlaySenacContext : DbContext
    {
        public PlaySenacContext (DbContextOptions<PlaySenacContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Product { get; set; } = default!;
        public DbSet<Seller> Seller { get; set; } = default!;
        public DbSet<Department> Department { get; set; } = default!;
        public DbSet<SalesRecord> SalesRecord { get; set; } = default!;
    }
}
