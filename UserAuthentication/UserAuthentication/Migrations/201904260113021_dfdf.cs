namespace UserAuthentication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dfdf : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Staffs", "DOB", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Staffs", "DOB", c => c.String(nullable: false));
        }
    }
}
