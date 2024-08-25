using DatabasesBenchmark.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DatabasesBenchmark.Infrastructure.DbContext
{
    public class MySqlBenchmarkDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public DbSet<Benchmark> Benchmarks { get; set; }

        public MySqlBenchmarkDbContext(DbContextOptions<MySqlBenchmarkDbContext> opts) : base(opts)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Benchmark>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.RandomString).IsRequired().HasMaxLength(255);
            });
        }
    }
}
