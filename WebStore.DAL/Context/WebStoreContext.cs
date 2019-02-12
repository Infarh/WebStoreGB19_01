﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using WebStore.Domain.Entities;

namespace WebStore.DAL.Context
{
    public class WebStoreContext : DbContext
    {
         public WebStoreContext(DbContextOptions<WebStoreContext> options) : base(options) { }

         public DbSet<Brand> Brands { get; set; }

         public DbSet<Section> Sections { get; set; }

         public DbSet<Product> Products { get; set; }
    }
}
