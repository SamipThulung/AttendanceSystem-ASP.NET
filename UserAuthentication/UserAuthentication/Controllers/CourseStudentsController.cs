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
    public class CourseStudentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: CourseStudents
        public ActionResult Index()
        {
           string index = "SELECT s.StudentID,cs.CourseID,cs.CompletedYear,cs.TotalMarks,c.Title AS CourseTitle,CONCAT(s.FirstName, ',', s.LastName) AS StudentName FROM coursestudents cs JOIN courses c ON c.CourseID = cs.CourseID JOIN students s ON s.StudentID = cs.StudentID";
            var dt = db.List(index);
            var model = new CourseStudent().List(dt);
            return View(model);
        }

        // GET: CourseStudents/Create
        public ActionResult Create()
        {
            //get Course
            String course = "SELECT * FROM courses";
            var dt = db.List(course);
            var cModel = new Course().List(dt);
            //get Student
            String student = "SELECT * FROM students s JOIN Groups g ON s.GroupID = g.GroupID";
            var dt2 = db.List(student);
            var sModel = new Student().List(dt2);

            ViewBag.CourseID = new SelectList(cModel, "CourseID", "Title");
            ViewBag.StudentID = new SelectList(sModel, "StudentID", "FirstName");
            return View();
        }

        // POST: CourseStudents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StudentID,CourseID,TotalMarks,CompletedYear")] CourseStudent courseStudent)
        {
            if (ModelState.IsValid)
            {
                db.CourseStudents.Add(courseStudent);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //get Course
            String course = "SELECT * FROM courses";
            var dt = db.List(course);
            var cModel = new Course().List(dt);
            //get Student
            String student = "SELECT * FROM students";
            var dt2 = db.List(student);
            var sModel = new Student().List(dt2);

            ViewBag.CourseID = new SelectList(cModel, "CourseID", "Title", courseStudent.CourseID);
            ViewBag.StudentID = new SelectList(sModel, "StudentID", "FirstName", courseStudent.StudentID);
            return View(courseStudent);
        }

        // GET: CourseStudents/Edit/5
        public ActionResult Edit(string c_id,string s_id)
        {
            if (c_id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            string index = "SELECT s.StudentID,cs.CourseID,cs.CompletedYear,cs.TotalMarks,c.Title AS CourseTitle,CONCAT(s.FirstName, ',', s.LastName) AS StudentName FROM coursestudents cs JOIN courses c ON c.CourseID = cs.CourseID JOIN students s ON s.StudentID = cs.StudentID Where c.CourseID='"+c_id+"' and cs.StudentID='"+s_id+"'";
            var dt = db.List(index);

           
            CourseStudent courseStudent = new CourseStudent().List(dt).FirstOrDefault();
            if (courseStudent == null)
            {
                return HttpNotFound();
            }
            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "Title", courseStudent.CourseID);
            ViewBag.StudentID = new SelectList(db.Students, "StudentID", "FirstName", courseStudent.StudentID);
            return View(courseStudent);
        }

        // POST: CourseStudents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StudentID,CourseID,TotalMarks,CompletedYear")] CourseStudent courseStudent)
        {
            if (ModelState.IsValid)
            {
                db.Entry(courseStudent).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "Title", courseStudent.CourseID);
            ViewBag.StudentID = new SelectList(db.Students, "StudentID", "FirstName", courseStudent.StudentID);
            return View(courseStudent);
        }

        // GET: CourseStudents/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CourseStudent courseStudent = db.CourseStudents.Find(id);
            if (courseStudent == null)
            {
                return HttpNotFound();
            }
            return View(courseStudent);
        }

        // POST: CourseStudents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            CourseStudent courseStudent = db.CourseStudents.Find(id);
            db.CourseStudents.Remove(courseStudent);
            db.SaveChanges();
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
