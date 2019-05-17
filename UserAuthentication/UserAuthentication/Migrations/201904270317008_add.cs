namespace UserAuthentication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Staffs", "Email", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Staffs", "Email");
        }
    }
}
