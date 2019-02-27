using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebStore.Domain.Entities;

namespace WebStore.DAL.Context
{
    public class WebStoreContext : IdentityDbContext<User>
    {
         public WebStoreContext(DbContextOptions<WebStoreContext> options) : base(options) { }

         public DbSet<Brand> Brands { get; set; }

         public DbSet<Section> Sections { get; set; }

         public DbSet<Product> Products { get; set; }

         public DbSet<Order> Orders { get; set; }

         public DbSet<OrderItem> OrderItems { get; set; }
    }
}
