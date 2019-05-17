using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UserAuthentication.Models
{
    [Authorize]
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Home
        public ActionResult Index()
        {
            String countTeacher = "SELECT COUNT(staffid) as ct FROM jobstaffs join jobs on jobstaffs.JobID = jobs.JobID and jobstaffs.JobID in (1,2,3)";
            var dt = db.List(countTeacher);
            var cTeacher = dt.Rows[0]["ct"].ToString();

            String countStudent = "SELECT COUNT(studentid) as cs FROM students";
            var dt2 = db.List(countStudent);
            var cStudent = dt2.Rows[0]["cs"].ToString();

            String countFaculty = "SELECT COUNT(facultyid) as cf FROM faculties";
            var dt3 = db.List(countFaculty);
            var cFaculty = dt3.Rows[0]["cf"].ToString();

            String countCourses = "SELECT COUNT(courseid) as cc FROM courses";
            var dt4 = db.List(countCourses);
            var cCourse = dt4.Rows[0]["cc"].ToString();


            ViewBag.ct = cTeacher;
            ViewBag.cs = cStudent;
            ViewBag.cf = cFaculty;
            ViewBag.cc = cCourse;

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}