using InfocommSolutionsProject.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using InfocommSolutionsProject.Models;

namespace InfocommSolutionsProject.Data;

public class InfocommSolutionsProjectContext : IdentityDbContext<Accounts>
{
    public InfocommSolutionsProjectContext(DbContextOptions<InfocommSolutionsProjectContext> options)
        : base(options)
    {

    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        
    }

    //public DbSet<Accounts> AccountTable { get; set; }
    public DbSet<ProductModel> Products { get; set; }

    public DbSet<OrdersModel> Orders { get; set; }

    public DbSet<PaymentModel> Payment { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }
}
