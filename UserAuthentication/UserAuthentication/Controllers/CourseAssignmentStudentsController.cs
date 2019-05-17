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
    public class CourseAssignmentStudentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: CourseAssignmentStudents
        public ActionResult Index()
        {
            String index = "SELECT * FROM courseassignmentstudents";
            var dt = db.List(index);
            var model = new CourseAssignmentStudent().List(dt);
            return View(model);
            //var courseAssignmentStudents = db.CourseAssignmentStudents.Include(c => c.assignment).Include(c => c.course);
            //return View(courseAssignmentStudents.ToList());
        }

        // GET: CourseAssignmentStudents/Create
        public ActionResult Create()
        {
            //get assignments
            String assignments = "SELECT * FROM assignments";
            var dt = db.List(assignments);
            var aModel = new Assignment().List(dt);
            //get courses
            String courses = "SELECT * FROM courses";
            var dt2 = db.List(courses);
            var cModel = new Course().List(dt2);

            ViewBag.AssignmentID = new SelectList(aModel, "AssignmentID", "CourseID");
            ViewBag.CourseID = new SelectList(cModel, "CourseID", "Title");
            return View();
        }

        // POST: CourseAssignmentStudents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CourseID,AssignmentID,TotalMarks")] CourseAssignmentStudent courseAssignmentStudent)
        {
            if (ModelState.IsValid)
            {
                db.CourseAssignmentStudents.Add(courseAssignmentStudent);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            //get assignments
            String assignments = "SELECT * FROM assignments";
            var dt = db.List(assignments);
            var aModel = new Assignment().List(dt);
            //get courses
            String courses = "SELECT * FROM courses";
            var dt2 = db.List(courses);
            var cModel = new Course().List(dt2);

            ViewBag.AssignmentID = new SelectList(aModel, "AssignmentID", "CourseID", courseAssignmentStudent.AssignmentID);
            ViewBag.CourseID = new SelectList(cModel, "CourseID", "Title", courseAssignmentStudent.CourseID);
            return View(courseAssignmentStudent);
        }

        // GET: CourseAssignmentStudents/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CourseAssignmentStudent courseAssignmentStudent = db.CourseAssignmentStudents.Find(id);
            if (courseAssignmentStudent == null)
            {
                return HttpNotFound();
            }
            ViewBag.AssignmentID = new SelectList(db.Assignments, "AssignmentID", "CourseID", courseAssignmentStudent.AssignmentID);
            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "Title", courseAssignmentStudent.CourseID);
            return View(courseAssignmentStudent);
        }

        // POST: CourseAssignmentStudents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CourseID,AssignmentID,TotalMarks")] CourseAssignmentStudent courseAssignmentStudent)
        {
            if (ModelState.IsValid)
            {
                db.Entry(courseAssignmentStudent).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AssignmentID = new SelectList(db.Assignments, "AssignmentID", "CourseID", courseAssignmentStudent.AssignmentID);
            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "Title", courseAssignmentStudent.CourseID);
            return View(courseAssignmentStudent);
        }

        // GET: CourseAssignmentStudents/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CourseAssignmentStudent courseAssignmentStudent = db.CourseAssignmentStudents.Find(id);
            if (courseAssignmentStudent == null)
            {
                return HttpNotFound();
            }
            return View(courseAssignmentStudent);
        }

        // POST: CourseAssignmentStudents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            CourseAssignmentStudent courseAssignmentStudent = db.CourseAssignmentStudents.Find(id);
            db.CourseAssignmentStudents.Remove(courseAssignmentStudent);
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
