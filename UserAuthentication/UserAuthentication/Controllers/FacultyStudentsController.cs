using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using UserAuthentication.Models;

namespace UserAuthentication.Controllers
{
    public class FacultyStudentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: FacultyStudents
        public ActionResult Index()
        {
            String sql = "SELECT * FROM FacultyStudents";
            db.List(sql);
            var dt = db.List(sql);
            var model = new FacultyStudent().List(dt);
            return View(model);
        }

        // GET: FacultyStudents/Details/5
        public ActionResult Details(string id)
        {

            String sql = "SELECT * FROM FacultyStudents WHERE FacultyID='" + id + "'";
            db.List(sql);
            var dt = db.List(sql);
            var model = new FacultyStudent().List(dt).FirstOrDefault();
            return View(model);
        }
        [Authorize(Roles ="Admin,Student Service")]
        // GET: FacultyStudents/Create
        public ActionResult Create()
        {
            //get Faculties
            String sql = "SELECT * FROM Faculties";
            db.List(sql);
            var dt = db.List(sql);

            //get Students
            String stu = "SELECT s.StudentID,s.FirstName,s.LastName,s.Gender,s.Email,s.ContactNumber,s.CurrentAddress,s.PermanentAddress,s.GroupID,s.EnrollDate,g.Semester FROM Students s JOIN GROUPS g ON s.GroupID=g.GroupID";

            db.List(stu);
            var datatable = db.List(stu);
            var faculty = new Faculty().List(dt);
            var student = new Student().List(datatable);
            ViewBag.FacultyID = new SelectList(faculty, "FacultyID", "FacultyID");
            ViewBag.StudentID = new SelectList(student, "StudentID", "StudentID");
            return View();

        }

        // POST: FacultyStudents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FacultyID,StudentID")] FacultyStudent facultyStudent)
        {
            if (ModelState.IsValid)
            {
                string create = "Insert Into FacultyStudents(FacultyID,StudentID) Values('" + facultyStudent.FacultyID + "','" + facultyStudent.StudentID + "')";
                db.Create(create);
                //db.FacultyStudents.Add(facultyStudent);
                //db.SaveChanges();
                return RedirectToAction("Index");
            }

            try
            {
                String sql = "SELECT * FROM Faculties";
                db.List(sql);
                var dt = db.List(sql);
                String stu = "SELECT s.StudentID,s.FirstName,s.LastName,s.Gender,s.Email,s.ContactNumber,s.CurrentAddress,s.PermanentAddress,s.GroupID,s.EnrollDate,g.Semester FROM Students s JOIN GROUPS g ON s.GroupID=g.GroupID";
                DataTable dddt = db.List(stu);



                var datatable = db.List(stu);
                var faculty = new Faculty().List(dt);
                var student = new Student().List(datatable);
                ViewBag.FacultyID = new SelectList(faculty, "FacultyID", "FacultyID");
                ViewBag.StudentID = new SelectList(student, "StudentID", "StudentID");
                return View(facultyStudent);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
                throw;
            }
           
        }

        

        // GET: FacultyStudents/Delete/5
        public ActionResult Delete(string Faculty_id, string Student_id)
        {
            System.Diagnostics.Debug.WriteLine(Faculty_id, Student_id);
            try
            {
                string sql = "Select * from FacultyStudents where FacultyID = '" + Faculty_id + "' and StudentID = '" + Student_id + "';";
                var dt = db.List(sql);
                var model = new FacultyStudent().List(dt).FirstOrDefault();
                return View(model);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
                throw;
            }
            
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            //FacultyStudent facultyStudent = db.FacultyStudents.Find(id);
            //if (facultyStudent == null)
            //{
            //    return HttpNotFound();
            //}
            //return View(facultyStudent);
        }

        // POST: FacultyStudents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id, FormCollection fm)
        {
            System.Diagnostics.Debug.WriteLine(fm["FacultyID"] + fm["StudentID"] );
            string sql = "Delete From FacultyStudents Where FacultyID='" + fm["FacultyID"] + "' and StudentID='" + fm["StudentID"] + "';";
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
