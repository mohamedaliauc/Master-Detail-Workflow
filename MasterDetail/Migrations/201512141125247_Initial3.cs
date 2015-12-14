namespace MasterDetail.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial3 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.WorkOrders", "OrderDateTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.WorkOrders", "OrderDateTime", c => c.DateTime(nullable: false));
        }
    }
}
