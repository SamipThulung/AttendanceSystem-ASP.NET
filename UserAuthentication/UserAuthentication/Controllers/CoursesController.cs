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
    public class CoursesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Courses
        public ActionResult Index()
        {
            String view = "SELECT * FROM courses";
            db.List(view);
            var dt = db.List(view);
            var model = new Course().List(dt);
            return View(model);
        }

        // GET: Courses/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            String view = "SELECT * FROM courses WHERE CourseID= '" + id + "'";
           
            var dt = db.List(view);
            Debug.WriteLine(dt.ToString());
            var model = new Course().List(dt).FirstOrDefault();
            return View(model);

        }

        // GET: Courses/Create
        public ActionResult Create()
        {
            string sql = "Select * From Faculties;";
            var a = db.List(sql);
            var FacultiesList = new Faculty().List(a);
            ViewBag.FacultyID = new SelectList(FacultiesList, "FacultyID", "Name");
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CourseID,Title,Period,Level,CreditHour")] Course course)
        {
            if (ModelState.IsValid)
            {
                String create = "INSERT INTO courses (CourseID, Title, Period, Level, CreditHour) VALUES ('" + course.CourseID + "' , '" + course.Title + "' , '" + course.Period + "' , '" + course.Level + "' , '" + course.CreditHour + "')";
                db.Create(create);
                //db.Courses.Add(course);
                //db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(course);
        }

        // GET: Courses/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            String view = "SELECT * FROM courses WHERE CourseID= '" + id + "'";

            var dt = db.List(view);
            Debug.WriteLine(dt.ToString());
            var model = new Course().List(dt).FirstOrDefault();
            return View(model);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CourseID,Title,Period,Level,CreditHour")] Course course)
        {
            if (ModelState.IsValid)
            {
                String edit = "UPDATE courses SET TITLE = '"+course.Title+"' , Period = '"+course.Period+"' , Level = '"+course.Level+"' , CreditHour = '"+course.CreditHour+"' WHERE CourseID = '"+course.CourseID+"' ";
                db.Edit(edit);
                //db.Entry(course).State = EntityState.Modified;
                //db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(course);
        }

        // GET: Courses/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            String view = "SELECT * FROM courses WHERE CourseID= '" + id + "'";

            var dt = db.List(view);
            Debug.WriteLine(dt.ToString());
            var model = new Course().List(dt).FirstOrDefault();
            return View(model);
            }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            String delete = "DELETE FROM courses WHERE CourseId = '" + id + "'";
            db.Delete(delete);
            //Course course = db.Courses.Find(id);
            //db.Courses.Remove(course);
            //db.SaveChanges();
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
