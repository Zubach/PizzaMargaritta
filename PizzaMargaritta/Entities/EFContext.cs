﻿using Microsoft.EntityFrameworkCore;
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

        public DbSet<Admin> Admins { get; set; }
        public DbSet<BasketPizza> pizzas_in_basket { get; set; }
    }
}
