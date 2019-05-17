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
    public class FacultyCoursesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: FacultyCourses
        public ActionResult Index()
        {
            String sql = "SELECT * FROM FacultyCourses";
            db.List(sql);
            var dt = db.List(sql);
            var model = new FacultyCourse().List(dt);
            return View(model);
            //var facultyCourses = db.FacultyCourses.Include(f => f.course).Include(f => f.Fac);
            //return View(facultyCourses.ToList());
        }

        // GET: FacultyCourses/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            String sql = "SELECT * FROM FacultyCourses WHERE FacultyID= '" + id + "'";
            db.List(sql);
            var dt = db.List(sql);
            var model = new FacultyCourse().List(dt).FirstOrDefault();
            return View(model);
            
        }

        // GET: FacultyCourses/Create
        public ActionResult Create()
        {
            String fac = "SELECT * FROM Faculties";
            db.List(fac);
            var dt = db.List(fac);
            String cou = "SELECT * FROM Courses";
            db.List(cou);
            var datatable = db.List(cou);
            var faculty = new Faculty().List(dt);
            var course = new Course().List(datatable);
            ViewBag.FacultyID = new SelectList(faculty, "FacultyID", "FacultyID");
            ViewBag.CourseID = new SelectList(course, "CourseID", "CourseID");
            return View();
        }

        // POST: FacultyCourses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FacultyID,CourseID")] FacultyCourse facultyCourse)
        {
            if (ModelState.IsValid)
            {
                string create = "Insert Into FacultyCourses(FacultyID,CourseID) Values('" + facultyCourse.FacultyID + "','" + facultyCourse.CourseID + "')";
                db.Create(create);
                //db.FacultyCourses.Add(facultyCourse);
                //db.SaveChanges();
                return RedirectToAction("Index");
            }

            String fac = "SELECT * FROM Faculties";
            db.List(fac);
            var dt = db.List(fac);
            String cou = "SELECT * FROM Courses";
            db.List(cou);
            var datatable = db.List(cou);
            var faculty = new Faculty().List(dt);
            var course = new Student().List(datatable);
            ViewBag.FacultyID = new SelectList(faculty, "FacultyID", "FacultyID");
            ViewBag.StudentID = new SelectList(course, "CourseID", "CourseID");
            return View(facultyCourse);
            //ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "Title", facultyCourse.CourseID);
            //ViewBag.FacultyID = new SelectList(db.Faculties, "FacultyID", "Name", facultyCourse.FacultyID);
            //return View(facultyCourse);
        }

        // GET: FacultyCourses/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FacultyCourse facultyCourse = db.FacultyCourses.Find(id);
            if (facultyCourse == null)
            {
                return HttpNotFound();
            }
            String fac = "SELECT * FROM Faculties";
            db.List(fac);
            var dt = db.List(fac);
            String cou = "SELECT * FROM Courses";
            db.List(cou);
            var datatable = db.List(cou);
            var faculty = new Faculty().List(dt);
            var course = new Student().List(datatable);
            ViewBag.FacultyID = new SelectList(faculty, "FacultyID", "FacultyID", facultyCourse.FacultyID);
            ViewBag.StudentID = new SelectList(course, "CourseID", "CourseID", facultyCourse.CourseID);
            return View(facultyCourse);
            ///ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "Title", facultyCourse.CourseID);
            //ViewBag.FacultyID = new SelectList(db.Faculties, "FacultyID", "Name", facultyCourse.FacultyID);
            //return View(facultyCourse);
        }

        // POST: FacultyCourses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FacultyID,CourseID")] FacultyCourse facultyCourse)
        {
            if (ModelState.IsValid)
            {
                db.Entry(facultyCourse).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            String fac = "SELECT * FROM Faculties";
            db.List(fac);
            var dt = db.List(fac);
            String cou = "SELECT * FROM Courses";
            db.List(cou);
            var datatable = db.List(cou);
            var faculty = new Faculty().List(dt);
            var course = new Student().List(datatable);
            ViewBag.FacultyID = new SelectList(faculty, "FacultyID", "FacultyID", facultyCourse.FacultyID);
            ViewBag.StudentID = new SelectList(course, "CourseID", "CourseID", facultyCourse.CourseID);
            return View(facultyCourse);
            //ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "Title", facultyCourse.CourseID);
            //ViewBag.FacultyID = new SelectList(db.Faculties, "FacultyID", "Name", facultyCourse.FacultyID);
            //return View(facultyCourse);
        }

        // GET: FacultyCourses/Delete/5
        public ActionResult Delete(string c_id,string f_id)
        {
            string sql = "DELETE FROM FacultyCourses WHERE CourseID= '" + c_id + "' and FacultyID = '" + f_id + "'";
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
