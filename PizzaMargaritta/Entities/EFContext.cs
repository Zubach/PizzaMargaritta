using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaMargaritta.Entities
{
    public class EFContext:DbContext
    {
       
        public EFContext(DbContextOptions<EFContext> options) :base(options)
        {

        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<Pizza> Pizzas { get; set; }

        public DbSet<User> Users { get; set; }
    }
}
