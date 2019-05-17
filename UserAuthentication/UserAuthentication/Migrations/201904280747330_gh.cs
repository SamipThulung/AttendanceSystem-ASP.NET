namespace UserAuthentication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class gh : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.CourseStudents", "TotalMarks", c => c.String());
            AlterColumn("dbo.CourseStudents", "CompletedYear", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CourseStudents", "CompletedYear", c => c.String(nullable: false));
            AlterColumn("dbo.CourseStudents", "TotalMarks", c => c.String(nullable: false));
        }
    }
}
