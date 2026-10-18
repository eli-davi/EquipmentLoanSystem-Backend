using EquipmentLoanSystem.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EquipmentLoanSystem.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Item> Items { get; set; }
        public DbSet<ItemCopy> ItemCopies { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Loan> Loans { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Item>()
                .HasMany(i => i.Categories)
                .WithMany(c => c.Items);

            modelBuilder.Entity<Item>()
                .HasMany(i => i.Copies)
                .WithOne(c => c.Item)
                .HasForeignKey(c => c.ItemId);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries<ItemCopy>()
                .Where(e => e.State == EntityState.Modified);

            foreach (var entry in entries)
            {
                entry.Entity.Version = Guid.NewGuid();
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}