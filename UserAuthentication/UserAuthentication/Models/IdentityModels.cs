using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Owin;

namespace UserAuthentication.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        //public void Configuration(IAppBuilder app)
        //{
         
        //    createRolesandUsers();
        //}


        // In this method we will create default User roles and Admin user for login   
        //private void createRolesandUsers()
        //{
        //    ApplicationDbContext context = new ApplicationDbContext();

        //    var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
        //    var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));


        //    // In Startup iam creating first Admin Role and creating a default Admin User    
        //    if (!roleManager.RoleExists("Admin"))
        //    {

        //        // first we create Admin rool   
        //        var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
        //        role.Name = "Admin";
        //        roleManager.Create(role);

        //        //Here we create a Admin super user who will maintain the website                  

        //        var user = new ApplicationUser();
        //        user.UserName = "admin";
        //        user.Email = "admin@gmail.com";

        //        string userPWD = "s@mIp123";

        //        var chkUser = UserManager.Create(user, userPWD);

        //        //Add default User to Role Admin   
        //        if (chkUser.Succeeded)
        //        {
        //            var result1 = UserManager.AddToRole(user.Id, "Admin");

        //        }
        //    }
        //    }
        }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {

        public DbSet<Faculty> Faculties { get; set; }

        public DbSet<Student> Students { get; set; }

        public DbSet<Group> Groups { get; set; }

        public DbSet<Course> Courses { get; set; }

        public DbSet<Staff> Staffs { get; set; }

        public DbSet<Schedule> Schedules { get; set; }

        public DbSet<Attendance> Attendances { get; set; }

        public DbSet<Assignment> Assignments { get; set; }

        public DbSet<Address> Addresses { get; set; }

        public DbSet<Job> Jobs { get; set; }

        public DbSet<Departments> Departments { get; set; }



        public DbSet<Institute> Institutes { get; set; }

        public DbSet<FacultyCourse> FacultyCourses { get; set; }

        public DbSet<FacultyStudent> FacultyStudents { get; set; }

        public DbSet<CourseStudent> CourseStudents { get; set; }

        public DbSet<StaffSchedule> StaffSchedules { get; set; }

        public DbSet<ScheduleGroup> ScheduleGroups { get; set; }

        public DbSet<DepartmentStaff> DepartmentStaffs { get; set; }

        public DbSet<JobStaff> JobStaffs { get; set; }

        public DbSet<StaffAddress> StaffAddresses { get; set; }

        public DbSet<CourseStaff> CourseStaffs { get; set; }

        public DbSet<StaffStudent> StaffStudents { get; set; }

        public DbSet<CourseAssignmentStudent> CourseAssignmentStudents { get; set; }
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {

            return new ApplicationDbContext();
        }
        public void Create(string sql)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            SqlCommand cmd = new SqlCommand(sql, con);

            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
            }
            finally
            {
                con.Close();
            }
            
        }
        public void Edit(string sql)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            SqlCommand cmd = new SqlCommand(sql, con);

            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
            }
            finally
            {
                con.Close();
            }

        }
        public void Delete(string sql)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            SqlCommand cmd = new SqlCommand(sql, con);

            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
            }
            finally
            {
                con.Close();
            }

        }
        public DataTable List(string sql)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            SqlCommand cmd = new SqlCommand(sql, con);

            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            try
            {
                con.Open();
                da.Fill(dt);
            }
            finally
            {
                con.Close();
            }
            return dt;
        }
    }
}