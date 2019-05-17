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
    public class StudentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Students
        public ActionResult Index()
        {
            String sql = "SELECT * FROM students INNER JOIN groups ON students.GroupID=groups.GroupID";
            db.List(sql);
            var dt = db.List(sql);
            var model = new Student().List(dt);
            return View(model);
            // var students = db.Students.Include(s => s.group);
            //eturn View(students.ToList());
        }

        // GET: Students/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            String sql = "SELECT * FROM students INNER JOIN groups ON students.GroupID = groups.GroupID WHERE StudentID= '" + id + "'";
            db.List(sql);
            var dt = db.List(sql);
            var model = new Student().List(dt).FirstOrDefault();
            return View(model);
        }

        // GET: Students/Create
        public ActionResult Create()
        {
            String sql = "SELECT g.GroupID,g.Semester,g.Year,g.FacultyId,f.Name FROM Groups g JOIN Faculties f ON f.facultyID=g.FacultyID";
            var dt = db.List(sql);
            var groups = new Group().List(dt);
            ViewBag.GroupID = new SelectList(groups, "GroupID", "GroupID");
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StudentID,FirstName,LastName,Gender,Email,ContactNumber,CurrentAddress,PermanentAddress,GroupID,EnrollDate")] Student student)
        {
            if (ModelState.IsValid)
            {
                string sql = "Insert Into Students(StudentID,FirstName,LastName,Gender,Email,ContactNumber,CurrentAddress,PermanentAddress,GroupID,EnrollDate) Values('" + student.StudentID + "','" + student.FirstName + "','" + student.LastName + "','" + student.Gender + "','" + student.Email + "','" + student.ContactNumber + "','" + student.CurrentAddress + "','" + student.PermanentAddress + "','" + student.GroupID + "','" + student.EnrollDate + "')";
                db.Create(sql);
                //db.Students.Add(student);
                //db.SaveChanges();
                return RedirectToAction("Index");
            }

            String sql_group = "SELECT * FROM groups INNER JOIN students ON groups.GroupID=students.GroupID";
            var dt = db.List(sql_group);
            var groups = new Student().List(dt);
            ViewBag.GroupID = new SelectList(groups, "GroupID", "GroupID");
            return View();
        }

        // GET: Students/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            String sql = "SELECT * FROM groups g JOIN Faculties f On f.FacultyID = g.GroupID";
            db.List(sql);
            var dt = db.List(sql);
            var groups = new Group().List(dt);
            ViewBag.GroupID = new SelectList(groups, "GroupID", "GroupID", student.GroupID);
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StudentID,FirstName,LastName,Gender,Email,ContactNumber,CurrentAddress,PermanentAddress,GroupID,EnrollDate")] Student student)
        {
            if (ModelState.IsValid)
            {
                string sql = "Update Students SET StudentID ='" + student.StudentID + "',FirstName='" + student.FirstName + "',LastName='" + student.LastName + "',Gender='" + student.Gender + "',Email='" + student.Email + "',ContactNumber='" + student.ContactNumber + "',CurrentAddress='" + student.CurrentAddress + "',PermanentAddress='" + student.PermanentAddress + "',GroupID='" + student.GroupID + "',EnrollDate='" + student.EnrollDate + "'  Where StudentID='" + student.StudentID + "'";
                db.Edit(sql);
                // db.Entry(student).State = EntityState.Modified;
                //db.SaveChanges();
                return RedirectToAction("Index");
            }
            String groupview = "SELECT * FROM groups";
            db.List(groupview);
            var dt = db.List(groupview);
            var groups = new Group().List(dt);
            ViewBag.GroupID = new SelectList(groups, "GroupID", "GroupID", student.GroupID);
            return View(student);
        }

        // GET: Students/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            String sql = "SELECT * FROM students s JOIN groups g ON g.GroupID = s.GroupID where StudentID= '" + id + "'";
            db.List(sql);
            var dt = db.List(sql);
            var model = new Student().List(dt).FirstOrDefault();
            return View(model);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            string sql = "DELETE FROM students WHERE StudentID= '" + id + "'";
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
