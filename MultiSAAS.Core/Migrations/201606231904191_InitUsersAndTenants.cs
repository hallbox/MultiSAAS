namespace MultiSAAS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitUsersAndTenants : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tenant",
                c => new
                    {
                        TenantCode = c.String(nullable: false, maxLength: 20),
                        TenantName = c.String(nullable: false, maxLength: 50),
                        InboundAccess = c.Boolean(nullable: false),
                        ConnectionString = c.String(maxLength: 200),
                        LogoImageData = c.String(),
                        CreatedBy = c.String(nullable: false, maxLength: 8),
                        CreatedDT = c.DateTime(nullable: false),
                        LastChangedBy = c.String(nullable: false, maxLength: 8),
                        LastChangedDT = c.DateTime(nullable: false),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.TenantCode);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        Username = c.String(nullable: false, maxLength: 8),
                        Password = c.String(maxLength: 32),
                        FirstName = c.String(nullable: false, maxLength: 25),
                        LastName = c.String(nullable: false, maxLength: 25),
                        EmailAddress = c.String(maxLength: 50),
                        ExternalTenantCode = c.String(maxLength: 20),
                        ExternalUsername = c.String(maxLength: 8),
                        Enabled = c.Boolean(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 8),
                        CreatedDT = c.DateTime(nullable: false),
                        LastChangedBy = c.String(nullable: false, maxLength: 8),
                        LastChangedDT = c.DateTime(nullable: false),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Username)
                .ForeignKey("dbo.Tenant", t => t.ExternalTenantCode)
                .Index(t => t.ExternalTenantCode);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.User", "ExternalTenantCode", "dbo.Tenant");
            DropIndex("dbo.User", new[] { "ExternalTenantCode" });
            DropTable("dbo.User");
            DropTable("dbo.Tenant");
        }
    }
}
