using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using UserAuthentication.Models;

namespace UserAuthentication.Controllers
{
    [Authorize(Roles = "Admin,Student Service")]
    public class CourseStaffsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: CourseStaffs
        public ActionResult Index()
        {
            String sql = "SELECT cs.CourseID,cs.StaffID,c.Title, s.FirstName ,s.LastName,j.Title as JobTitle FROM CourseStaffs cs JOIN Courses c ON c.CourseID = cs.CourseID JOIN Staffs s ON s.StaffID = cs.StaffID JOIN JobStaffs js ON js.StaffID = s.StaffID JOIN Jobs j ON j.JobID = js.JobID Where j.Title IN ('Teacher','Lecturer','Tutor','Module Leader')";
            var dt = db.List(sql);
            var model = new CourseStaff().List(dt);

            string coursesql = "Select * from Schedules s JOIN Courses c ON c.CourseID=s.CourseID";
            var d = db.List(coursesql);
            var courseModel = new Schedule().List(d);

            ViewBag.CourseID = new SelectList(courseModel, "CourseID", "CourseID");

            for (int i = 0; i < model.Count; i++)
            {
                Debug.WriteLine(model[i].CourseID);
                Debug.WriteLine(model[i].StaffID);
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(string CourseID)
        {
            String sql = "SELECT cs.CourseID,cs.StaffID,c.Title, s.FirstName ,s.LastName,j.Title as JobTitle FROM CourseStaffs cs JOIN Courses c ON c.CourseID = cs.CourseID JOIN Staffs s ON s.StaffID = cs.StaffID JOIN JobStaffs js ON js.StaffID = s.StaffID JOIN Jobs j ON j.JobID = js.JobID Where j.Title IN ('Teacher','Lecturer','Tutor','Module Leader') and c.CourseID = '"+CourseID+"'";
            var dt = db.List(sql);
            var model = new CourseStaff().List(dt);

            string coursesql = "Select * from Courses";
            var d = db.List(coursesql);
            var courseModel = new Course().List(d);

            ViewBag.CourseID = new SelectList(courseModel, "CourseID", "CourseID");
          
            return View(model);
        }

        

        // GET: CourseStaffs/Create
        public ActionResult Create()
        {
            // get Courses
             String sql = "SELECT * FROM Courses";
            var dt = db.List(sql);

            // get Staffs
            String sql_course = "SELECT * FROM Staffs";
            var dataTable = db.List(sql_course);

            var course = new Course().List(dt);
            var staff = new Staff().List(dataTable);
            ViewBag.CourseID = new SelectList(course, "CourseID", "Title");
            ViewBag.StaffID = new SelectList(staff, "StaffID", "StaffDetail");
            return View();
        }

        // POST: CourseStaffs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CourseStaff courseStaff)
        {
            if (ModelState.IsValid)
            {
                string create = "Insert Into CourseStaffs(CourseID,StaffID,Semester) Values('" + courseStaff.CourseID + "','" + courseStaff.StaffID + "','" + courseStaff.Semester + "')";
                db.Create(create);
                return RedirectToAction("Index");
            }

            try
            {
                String sql = "SELECT * FROM Courses";
                var dt = db.List(sql);

                String stf = "SELECT * FROM Staffs";
                var dt_staff = db.List(stf);
                var course = new Course().List(dt);
                var staff = new Staff().List(dt_staff);
                ViewBag.CourseID = new SelectList(course, "CourseID", "CourseID");
                ViewBag.StaffID = new SelectList(staff, "StaffID", "StaffID");
                return View(courseStaff);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
                throw;
            }
        }

        

        // GET: CourseStaffs/Delete/5
        public ActionResult Delete()
        {
            Debug.WriteLine("Delete intoer");
            Debug.WriteLine(Request.QueryString["s_id"]);
            Debug.WriteLine(Request.QueryString["c_id"]);
            string c_id = Request.QueryString["c_id"];
            string s_id = Request.QueryString["s_id"];

            Debug.WriteLine(c_id+ s_id);
            try
            {
                string sql = "Select c.CourseID,s.StaffID,c.Title,j.Title AS JobTitle,s.StaffID,s.FirstName,s.LastName from CourseStaffs cs JOIN Courses c on c.CourseID=cs.CourseID JOIN Staffs s ON s.StaffID=cs.StaffID JOIN JobStaffs js ON js.StaffID = s.StaffID JOIN Jobs j ON j.JobID = js.JobID  where c.CourseID = '" + c_id + "' and s.StaffID = '" + s_id + "';";
                var dt = db.List(sql);
                Debug.WriteLine(dt.Rows.Count);
                var model = new CourseStaff().List(dt).FirstOrDefault();
           
                return View(model);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
                throw;
            }
        }

        // POST: CourseStaffs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id, FormCollection fm)
        {
            System.Diagnostics.Debug.WriteLine(fm["CourseID"] + fm["StaffID"]);
            string sql = "Delete From CourseStaffs Where CourseID='" + fm["CourseID"] + "' and StaffID='" + fm["StaffID"] + "';";
            db.Delete(sql);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
