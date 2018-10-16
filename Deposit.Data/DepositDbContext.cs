using Deposit.Domain.Configuration;
using Microsoft.EntityFrameworkCore;
using Deposit.Domain.Entities;

namespace Deposit.Data
{
    public class DepositDbContext : DbContext
    {
        public DepositDbContext(DbContextOptions<DepositDbContext> options) : base(options) 
        {}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new ProductConfiguration());
        }
        
        public DbSet<Product> Products { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerOrder> CustomerOrders { get; set; }
        public DbSet<CustomerOrderItem> CustomerOrderItems { get; set; }
        public DbSet<CustomerDeposit> CustomerDeposits { get; set; }
        public DbSet<Provider> Providers { get; set; }
        public DbSet<ProviderOrder> ProviderOrders { get; set; }
        public DbSet<ProviderOrderItem> ProviderOrderItems { get; set; }
        public DbSet<ProviderDeposit> ProviderDeposits { get; set; }
    }
}