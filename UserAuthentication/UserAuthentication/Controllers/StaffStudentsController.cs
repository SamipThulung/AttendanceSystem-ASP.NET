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
    public class StaffStudentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: StaffStudents
        public ActionResult Index()
        {
            String sql = "SELECT * FROM StaffStudents";
            db.List(sql);
            var dt = db.List(sql);
            var model = new StaffStudent().List(dt);
            return View(model);
            //var staffStudents = db.StaffStudents.Include(s => s.staff).Include(s => s.student);
            //return View(staffStudents.ToList());
        }

        // GET: StaffStudents/Details/5
        public ActionResult Details(string id)
        {
            String sql = "SELECT * FROM StaffStudents WHERE StaffID='" + id + "'";
            db.List(sql);
            var dt = db.List(sql);
            var model = new StaffStudent().List(dt).FirstOrDefault();
            return View(model);
        }

        // GET: StaffStudents/Create
        public ActionResult Create()
        {
            String sql = "SELECT * FROM Staffs";

            var dt = db.List(sql);
            
            String stu = "SELECT s.StudentID,s.FirstName,s.LastName,s.Gender,s.Email,s.ContactNumber,s.CurrentAddress,s.PermanentAddress,s.GroupID,s.EnrollDate,g.Semester FROM Students s JOIN GROUPS g ON s.GroupID=g.GroupID";

            db.List(stu);
            
            var datatable = db.List(stu);
            Debug.WriteLine(datatable.Rows.Count);
            var staff = new Staff().List(dt);
            var student = new Student().List(datatable);
            ViewBag.StaffID = new SelectList(staff, "StaffID", "StaffID");
            ViewBag.StudentID = new SelectList(student, "StudentID", "StudentID");
            return View();
        }

        // POST: StaffStudents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Bind(Include = "StaffID,StudentID")]
        public ActionResult Create(StaffStudent staffStudent)
        {
            if (ModelState.IsValid)
            {
                string create = "Insert Into StaffStudents(StaffID,StudentID) Values('" + staffStudent.StaffID + "','" + staffStudent.StudentID + "')";
                db.Create(create);
                //db.StaffStudents.Add(staffStudent);
                //db.SaveChanges();
                return RedirectToAction("Index");
            }
            try
            {
                String sql = "SELECT * FROM Staffs";
                db.List(sql);
                var dt = db.List(sql);
                String stu = "SELECT s.StudentID,s.FirstName,s.LastName,s.Gender,s.Email,s.ContactNumber,s.CurrentAddress,s.PermanentAddress,s.GroupID,s.EnrollDate,g.Semester FROM Students s JOIN GROUPS g ON s.GroupID=g.GroupID";
                DataTable dddt = db.List(stu);

                var datatable = db.List(stu);
                var staff = new Staff().List(dt);
                var student = new Student().List(datatable);
                ViewBag.StaffID = new SelectList(staff, "StafID", "StaffID");
                ViewBag.StudentID = new SelectList(student, "StudentID", "StudentID");
                return View(staffStudent);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
                throw;
            }
            //String staff = "SELECT * FROM staffs";
            //db.List(staff);
            //var dtStaff = db.List(staff);
            //var Staffs = new Staff().List(dtStaff);
            //String student = "SELECT * FROM students";
            //db.List(student);
            //var dtStudent = db.List(student);
            //var Students = new Student().List(dtStudent);
            //ViewBag.StaffID = new SelectList(Staffs, "StaffID", "FirstName");
            //ViewBag.StudentID = new SelectList(db.Students, "StudentID", "FirstName");
            //return View(staffStudent);
        }

        // GET: StaffStudents/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StaffStudent staffStudent = db.StaffStudents.Find(id);
            if (staffStudent == null)
            {
                return HttpNotFound();
            }
            ViewBag.StaffID = new SelectList(db.Staffs, "StaffID", "FirstName", staffStudent.StaffID);
            ViewBag.StudentID = new SelectList(db.Students, "StudentID", "FirstName", staffStudent.StudentID);
            return View(staffStudent);
        }

        // POST: StaffStudents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StaffID,StudentID")] StaffStudent staffStudent)
        {
            if (ModelState.IsValid)
            {
                db.Entry(staffStudent).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.StaffID = new SelectList(db.Staffs, "StaffID", "FirstName", staffStudent.StaffID);
            ViewBag.StudentID = new SelectList(db.Students, "StudentID", "FirstName", staffStudent.StudentID);
            return View(staffStudent);
        }

        // GET: StaffStudents/Delete/5
        public ActionResult Delete(string Staff_id, string Student_id)
        {
            System.Diagnostics.Debug.WriteLine(Staff_id, Student_id);
            try
            {
                string sql = "Select * from StaffStudents WHERE StaffID = '" + Staff_id + "' and StudentID = '" + Student_id + "';";
                var dt = db.List(sql);
                var model = new StaffStudent().List(dt).FirstOrDefault();
                return View(model);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
                throw;
            }
        }

        // POST: StaffStudents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id, FormCollection fm)
        {
            System.Diagnostics.Debug.WriteLine(fm["StaffID"] + fm["StudentID"]);
            string sql = "Delete From StaffStudents WHERE StaffID='" + fm["StaffID"] + "' and StudentID='" + fm["StudentID"] + "';";
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
