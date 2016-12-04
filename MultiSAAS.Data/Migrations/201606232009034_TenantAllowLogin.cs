namespace MultiSAAS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TenantAllowLogin : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tenant", "AllowLogin", c => c.Boolean(nullable: false));
            DropColumn("dbo.Tenant", "InboundAccess");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Tenant", "InboundAccess", c => c.Boolean(nullable: false));
            DropColumn("dbo.Tenant", "AllowLogin");
        }
    }
}
