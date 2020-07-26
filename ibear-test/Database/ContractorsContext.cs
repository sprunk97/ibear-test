using Microsoft.EntityFrameworkCore;
using System;

namespace ibear_test.Database
{
    class ContractorsContext : DbContext
    {
        public DbSet<Contractor> Contractors { get; set; }

        public ContractorsContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite("Data Source=Contractors.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Contractor>().HasData(
                new Contractor { ID = Guid.NewGuid(), Name = "Иван", Phone = 88005553535 },
                new Contractor { ID = Guid.NewGuid(), Name = "Сергей", Phone = 88005553534 },
                new Contractor { ID = Guid.NewGuid(), Name = "Андрей", Phone = 88005553533, Email = "andrey@mail.ru" });
        }
    }
}
