using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using KAndJCore.Models;

namespace KAndJCore.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<KAndJCore.Models.DocumentType> DocumentType { get; set; }
        public DbSet<KAndJCore.Models.Document> Document { get; set; }
        public DbSet<KAndJCore.Models.Client> Client { get; set; }
        public DbSet<KAndJCore.Models.AccountType> AccountType { get; set; }
        public DbSet<KAndJCore.Models.Account> Account { get; set; }
        public DbSet<KAndJCore.Models.Buro> Buro { get; set; }
        public DbSet<KAndJCore.Models.Template> Template { get; set; }
        public DbSet<KAndJCore.Models.Dispute> Dispute { get; set; }
        public DbSet<KAndJCore.Models.Claim> Claim { get; set; }
        public DbSet<KAndJCore.Models.Reason> Reason { get; set; }
    }
}
