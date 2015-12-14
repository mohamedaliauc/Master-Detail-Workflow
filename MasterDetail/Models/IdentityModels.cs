using MasterDetail.DataLayer;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace MasterDetail.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
        }
        string fun = @"ALTER Function [dbo].[GetSumOfPartsAndLabor](@workOrderId INT)
                RETURNS DECIMAL(18,2)
                AS
                BEGIN
                DECLARE @partsSum Decimal(18,2);
                DECLARE @laborSum Decimal(18,2);
                SELECT @partsSum=Sum(ExtendedPrice)
                From Parts
                Where workOrderId=@workOrderId;
                SELECT @laborSum=Sum(ExtendedPrice)
                From Labors
                Where workOrderId=@workOrderId;
                Return ISNULL(@partsSum,0)+ISNULL(@laborSum,0);
                END";
        public DbSet<Category> Categories { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<InventoryItem> InventoryItems { get; set; }
        public DbSet<Labor> Labors { get; set; }
        public DbSet<Part> Parts { get; set; }
        public DbSet<ServiceItem> ServiceItems { get; set; }
        public DbSet<WorkOrder> WorkOrders { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new CategoryConfiguration() );
            modelBuilder.Configurations.Add(new CustomerConfiguration());
            modelBuilder.Configurations.Add(new InventoryItemConfiguration());
            modelBuilder.Configurations.Add(new LaborConfiguration());
            modelBuilder.Configurations.Add(new PartConfiguration());
            modelBuilder.Configurations.Add(new ServiceItemConfiguration());
            modelBuilder.Configurations.Add(new WorkOrderConfiguration());

            base.OnModelCreating(modelBuilder);
        }
        

    }
}