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
    public class DepartmentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Departments
        public ActionResult Index()
        {
            String view = "SELECT * FROM departments";
            db.List(view);
            var dt = db.List(view);
            var model = new Departments().List(dt);
            return View(model);
        }

        // GET: Departments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            String view = "SELECT * FROM departments WHERE DepartmentID= '" + id + "'";

            var dt = db.List(view);
            var model = new Departments().List(dt).FirstOrDefault();
            return View(model);
        }

        // GET: Departments/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Departments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DepartmentID,DepartmentName")] Departments departments)
        {
            if (ModelState.IsValid)
            {
                String create = "INSERT INTO departments (DepartmentName) VALUES ('" + departments.DepartmentName + "')";
                db.Create(create);
                //db.Courses.Add(course);
                //db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(departments);
        }

        // GET: Departments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            String view = "SELECT * FROM departments WHERE DepartmentID= '" + id + "'";

            var dt = db.List(view);
            var model = new Departments().List(dt).FirstOrDefault();
            return View(model);
        }

        // POST: Departments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DepartmentID,DepartmentName")] Departments departments)
        {
            if (ModelState.IsValid)
            {
                String edit = "UPDATE departments SET DepartmentName = '" + departments.DepartmentName + "' WHERE DepartmentID = '" + departments.DepartmentID + "' ";
                db.Edit(edit);
                //db.Entry(course).State = EntityState.Modified;
                //db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(departments);
        }

        // GET: Departments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            String view = "SELECT * FROM departments WHERE DepartmentID= '" + id + "'";

            var dt = db.List(view);
            var model = new Departments().List(dt).FirstOrDefault();
            return View(model);
        }

        // POST: Departments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            String delete = "DELETE FROM departments WHERE DepartmentID = '" + id + "'";
            db.Delete(delete);
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
