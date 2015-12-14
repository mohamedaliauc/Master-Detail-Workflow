namespace MasterDetail.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initi : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        CategoryId = c.Int(nullable: false, identity: true),
                        CategoryName = c.String(nullable: false, maxLength: 20),
                    })
                .PrimaryKey(t => t.CategoryId)
                .Index(t => t.CategoryName, unique: true, name: "AK_Category_CategoryName");
            
            CreateTable(
                "dbo.InventoryItems",
                c => new
                    {
                        InventoryItemId = c.Int(nullable: false, identity: true),
                        InventoryItemCode = c.String(nullable: false, maxLength: 80),
                        InventoryItemName = c.String(),
                        UnitPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CategoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.InventoryItemId)
                .ForeignKey("dbo.Categories", t => t.CategoryId, cascadeDelete: true)
                .Index(t => t.InventoryItemCode, unique: true, name: "AK_InventoryItem_InventoryItemName")
                .Index(t => t.CategoryId);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        CustomerId = c.Int(nullable: false, identity: true),
                        AccountNumber = c.String(nullable: false, maxLength: 8),
                        CompanyName = c.String(nullable: false, maxLength: 30),
                        Address = c.String(nullable: false, maxLength: 30),
                        City = c.String(nullable: false, maxLength: 15),
                        State = c.String(nullable: false, maxLength: 2),
                        ZipCode = c.String(nullable: false, maxLength: 10),
                        Phone = c.String(maxLength: 15),
                    })
                .PrimaryKey(t => t.CustomerId)
                .Index(t => t.AccountNumber, unique: true, name: "AK_Customer_AccountNumber");
            
            CreateTable(
                "dbo.Labors",
                c => new
                    {
                        LaborId = c.Int(nullable: false, identity: true),
                        WorkOrderId = c.Int(nullable: false),
                        ServiceItemCode = c.String(nullable: false, maxLength: 15),
                        ServiceItemName = c.String(nullable: false, maxLength: 80),
                        LaborHours = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Rate = c.Decimal(nullable: false, precision: 18, scale: 2),
                       // ExtendedPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Notes = c.String(maxLength: 140),
                    })
                .PrimaryKey(t => t.LaborId)
                .ForeignKey("dbo.WorkOrders", t => t.WorkOrderId, cascadeDelete: true)
                .Index(t => new { t.WorkOrderId, t.ServiceItemCode }, unique: true, name: "AK_Labor");
            Sql("ALTER TABLE dbo.Labors ADD ExtendedPrice AS CAST(LaborHours * Rate AS Decimal(18,2))");
            CreateTable(
                "dbo.WorkOrders",
                c => new
                    {
                        WorkOrderId = c.Int(nullable: false, identity: true),
                        CustomerId = c.Int(nullable: false),
                        OrderDateTime = c.DateTime(nullable: false,defaultValueSql:"GetDate()"),
                        TargetDateTime = c.DateTime(),
                        DropDeadDateTime = c.DateTime(),
                        Description = c.String(maxLength: 256),
                        WorkOrderStatus = c.Int(nullable: false),
                        //Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CertificationRequirements = c.String(maxLength: 120),
                    })
                .PrimaryKey(t => t.WorkOrderId)
                .ForeignKey("dbo.Customers", t => t.CustomerId, cascadeDelete: true)
                .Index(t => t.CustomerId);
            Sql(@"Create Function dbo.GetSumOfPartsAndLabor(@workOrderId INT)
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
                END");
            Sql("ALTER TABLE dbo.WorkOrders ADD Total As dbo.GetSumOfPartsAndLabor(workOrderId)");
            CreateTable(
                "dbo.Parts",
                c => new
                    {
                        PartId = c.Int(nullable: false, identity: true),
                        WorkOrderId = c.Int(nullable: false),
                        InventoryItemCode = c.String(nullable: false, maxLength: 80),
                        InventoryItemName = c.String(),
                        Quantity = c.Int(nullable: false),
                        UnitPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        //ExtendedPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Notes = c.String(maxLength: 140),
                        IsInstalled = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.PartId)
                .ForeignKey("dbo.WorkOrders", t => t.WorkOrderId, cascadeDelete: true)
                .Index(t => new { t.WorkOrderId, t.InventoryItemCode }, unique: true, name: "AK_Part");
            Sql("ALTER TABLE dbo.Parts ADD ExtendedPrice AS CAST(Quantity * UnitPrice AS Decimal(18,2))");
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ServiceItems",
                c => new
                    {
                        ServiceItemId = c.Int(nullable: false, identity: true),
                        ServiceItemCode = c.String(nullable: false, maxLength: 15),
                        ServiceItemName = c.String(nullable: false, maxLength: 80),
                        Rate = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.ServiceItemId)
                .Index(t => t.ServiceItemCode, unique: true, name: "AK_ServiceItem_ServiceItemCode")
                .Index(t => t.ServiceItemName, unique: true, name: "AK_ServiceItem_ServiceItemName");
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        UserName = c.String(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                        User_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.LoginProvider, t.ProviderKey })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserClaims", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Parts", "WorkOrderId", "dbo.WorkOrders");
            DropForeignKey("dbo.Labors", "WorkOrderId", "dbo.WorkOrders");
            DropForeignKey("dbo.WorkOrders", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.InventoryItems", "CategoryId", "dbo.Categories");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "User_Id" });
            DropIndex("dbo.ServiceItems", "AK_ServiceItem_ServiceItemName");
            DropIndex("dbo.ServiceItems", "AK_ServiceItem_ServiceItemCode");
            DropIndex("dbo.Parts", "AK_Part");
            DropIndex("dbo.WorkOrders", new[] { "CustomerId" });
            DropIndex("dbo.Labors", "AK_Labor");
            DropIndex("dbo.Customers", "AK_Customer_AccountNumber");
            DropIndex("dbo.InventoryItems", new[] { "CategoryId" });
            DropIndex("dbo.InventoryItems", "AK_InventoryItem_InventoryItemName");
            DropIndex("dbo.Categories", "AK_Category_CategoryName");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.ServiceItems");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Parts");
            DropTable("dbo.WorkOrders");
            DropTable("dbo.Labors");
            DropTable("dbo.Customers");
            DropTable("dbo.InventoryItems");
            DropTable("dbo.Categories");
        }
    }
}
