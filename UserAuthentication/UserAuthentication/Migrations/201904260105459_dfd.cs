namespace UserAuthentication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dfd : DbMigration
    {
        public override void Up()
        {
            
            
            CreateTable(
                "dbo.Staffs",
                c => new
                    {
                        StaffID = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(nullable: false),
                        LastName = c.String(nullable: false),
                        ContactNo = c.String(nullable: false),
                        DOB = c.String(nullable: false),
                        JobID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.StaffID)
                .ForeignKey("dbo.Jobs", t => t.JobID, cascadeDelete: true)
                .Index(t => t.JobID);
            
            
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.StaffStudents", "StudentID", "dbo.Students");
            DropForeignKey("dbo.StaffStudents", "StaffID", "dbo.Staffs");
            DropForeignKey("dbo.StaffSchedules", "StaffID", "dbo.Staffs");
            DropForeignKey("dbo.StaffSchedules", "ScheduleID", "dbo.Schedules");
            DropForeignKey("dbo.StaffSchedules", "CourseID", "dbo.Courses");
            DropForeignKey("dbo.StaffAddresses", "StaffID", "dbo.Staffs");
            DropForeignKey("dbo.StaffAddresses", "AddressID", "dbo.Addresses");
            DropForeignKey("dbo.ScheduleGroups", "ScheduleID", "dbo.Schedules");
            DropForeignKey("dbo.ScheduleGroups", "GroupID", "dbo.Groups");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.JobStaffs", "StaffID", "dbo.Staffs");
            DropForeignKey("dbo.JobStaffs", "JobID", "dbo.Jobs");
            DropForeignKey("dbo.FacultyStudents", "StudentID", "dbo.Students");
            DropForeignKey("dbo.FacultyStudents", "FacultyID", "dbo.Faculties");
            DropForeignKey("dbo.FacultyCourses", "FacultyID", "dbo.Faculties");
            DropForeignKey("dbo.FacultyCourses", "CourseID", "dbo.Courses");
            DropForeignKey("dbo.DepartmentStaffs", "StaffID", "dbo.Staffs");
            DropForeignKey("dbo.DepartmentStaffs", "DepartmentID", "dbo.Departments");
            DropForeignKey("dbo.CourseStudents", "StudentID", "dbo.Students");
            DropForeignKey("dbo.CourseStudents", "CourseID", "dbo.Courses");
            DropForeignKey("dbo.CourseStaffs", "StaffID", "dbo.Staffs");
            DropForeignKey("dbo.Staffs", "JobID", "dbo.Jobs");
            DropForeignKey("dbo.CourseStaffs", "CourseID", "dbo.Courses");
            DropForeignKey("dbo.CourseAssignmentStudents", "CourseID", "dbo.Courses");
            DropForeignKey("dbo.CourseAssignmentStudents", "AssignmentID", "dbo.Assignments");
            DropForeignKey("dbo.Attendances", "StudentID", "dbo.Students");
            DropForeignKey("dbo.Students", "GroupID", "dbo.Groups");
            DropForeignKey("dbo.Groups", "FacultyID", "dbo.Faculties");
            DropForeignKey("dbo.Attendances", "ScheduleID", "dbo.Schedules");
            DropForeignKey("dbo.Schedules", "CourseID", "dbo.Courses");
            DropForeignKey("dbo.Attendances", "CourseID", "dbo.Courses");
            DropForeignKey("dbo.Assignments", "CourseID", "dbo.Courses");
            DropForeignKey("dbo.Courses", "FacultyID", "dbo.Faculties");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.StaffStudents", new[] { "StudentID" });
            DropIndex("dbo.StaffStudents", new[] { "StaffID" });
            DropIndex("dbo.StaffSchedules", new[] { "ScheduleID" });
            DropIndex("dbo.StaffSchedules", new[] { "CourseID" });
            DropIndex("dbo.StaffSchedules", new[] { "StaffID" });
            DropIndex("dbo.StaffAddresses", new[] { "AddressID" });
            DropIndex("dbo.StaffAddresses", new[] { "StaffID" });
            DropIndex("dbo.ScheduleGroups", new[] { "GroupID" });
            DropIndex("dbo.ScheduleGroups", new[] { "ScheduleID" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.JobStaffs", new[] { "JobID" });
            DropIndex("dbo.JobStaffs", new[] { "StaffID" });
            DropIndex("dbo.FacultyStudents", new[] { "StudentID" });
            DropIndex("dbo.FacultyStudents", new[] { "FacultyID" });
            DropIndex("dbo.FacultyCourses", new[] { "CourseID" });
            DropIndex("dbo.FacultyCourses", new[] { "FacultyID" });
            DropIndex("dbo.DepartmentStaffs", new[] { "StaffID" });
            DropIndex("dbo.DepartmentStaffs", new[] { "DepartmentID" });
            DropIndex("dbo.CourseStudents", new[] { "CourseID" });
            DropIndex("dbo.CourseStudents", new[] { "StudentID" });
            DropIndex("dbo.Staffs", new[] { "JobID" });
            DropIndex("dbo.CourseStaffs", new[] { "CourseID" });
            DropIndex("dbo.CourseStaffs", new[] { "StaffID" });
            DropIndex("dbo.CourseAssignmentStudents", new[] { "AssignmentID" });
            DropIndex("dbo.CourseAssignmentStudents", new[] { "CourseID" });
            DropIndex("dbo.Groups", new[] { "FacultyID" });
            DropIndex("dbo.Students", new[] { "GroupID" });
            DropIndex("dbo.Schedules", new[] { "CourseID" });
            DropIndex("dbo.Attendances", new[] { "StudentID" });
            DropIndex("dbo.Attendances", new[] { "CourseID" });
            DropIndex("dbo.Attendances", new[] { "ScheduleID" });
            DropIndex("dbo.Courses", new[] { "FacultyID" });
            DropIndex("dbo.Assignments", new[] { "CourseID" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.StaffStudents");
            DropTable("dbo.StaffSchedules");
            DropTable("dbo.StaffAddresses");
            DropTable("dbo.ScheduleGroups");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.JobStaffs");
            DropTable("dbo.Institutes");
            DropTable("dbo.FacultyStudents");
            DropTable("dbo.FacultyCourses");
            DropTable("dbo.DepartmentStaffs");
            DropTable("dbo.Departments");
            DropTable("dbo.CourseStudents");
            DropTable("dbo.Jobs");
            DropTable("dbo.Staffs");
            DropTable("dbo.CourseStaffs");
            DropTable("dbo.CourseAssignmentStudents");
            DropTable("dbo.Groups");
            DropTable("dbo.Students");
            DropTable("dbo.Schedules");
            DropTable("dbo.Attendances");
            DropTable("dbo.Faculties");
            DropTable("dbo.Courses");
            DropTable("dbo.Assignments");
            DropTable("dbo.Addresses");
        }
    }
}
