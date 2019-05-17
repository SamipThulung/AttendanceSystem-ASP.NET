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
    public class FacultiesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Faculties
        public ActionResult Index()
        {
            String sql = "SELECT * FROM Faculties ";
            db.List(sql);
            var dt = db.List(sql);
            var model = new Faculty().List(dt);
            return View(model);
        }

        // GET: Faculties/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            String sql = "SELECT * FROM faculties WHERE FacultyID= '" + id+"'";
            db.List(sql);
            var dt = db.List(sql);
            var model = new Faculty().List(dt).FirstOrDefault();
            return View(model);
        }

        // GET: Faculties/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Faculties/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FacultyID,Name,Description")] Faculty faculty)
        {
            if (ModelState.IsValid)
            {
                string sql = "Insert Into Faculties(FacultyID,Name,Description) Values('" + faculty.FacultyID + "','" + faculty.Name + "','" + faculty.Description + "')";
                db.Create(sql);
                //db.Faculties.Add(faculty);
                //db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(faculty);
        }

        // GET: Faculties/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Faculty faculty = db.Faculties.Find(id);
            if (faculty == null)
            {
                return HttpNotFound();
            }
            return View(faculty);
        }

        // POST: Faculties/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FacultyID,Name,Description")] Faculty faculty)
        {
            if (ModelState.IsValid)
            {
                string sql = "Update Faculties SET Name ='" + faculty.Name + "',Description='" + faculty.Description + "' Where FacultyID='" + faculty.FacultyID+"'";
                db.Edit(sql);
                //db.Entry(faculty).State = EntityState.Modified;
                //db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(faculty);
        }

        // GET: Faculties/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            String sql = "SELECT * FROM faculties where FacultyID= '" + id +"'";
            db.List(sql);
            var dt = db.List(sql);
            var model = new Faculty().List(dt).FirstOrDefault();
            return View(model);
        }

        // POST: Faculties/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            string sql = "DELETE FROM faculties WHERE facultyid= '" + id + "'";
            db.Delete(sql);
            //Faculty faculty = db.Faculties.Find(id);
            //db.Faculties.Remove(faculty);
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
