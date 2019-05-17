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
    public class JobsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Jobs
        public ActionResult Index()
        {
            String view = "SELECT * FROM jobs";
            db.List(view);
            var dt = db.List(view);
            var model = new Job().List(dt);
            return View(model);
        }

        // GET: Jobs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            String view = "SELECT * FROM jobs WHERE JobID= '" + id + "'";

            var dt = db.List(view);
            var model = new Job().List(dt).FirstOrDefault();
            return View(model);
        }

        // GET: Jobs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Jobs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "JobID,Title")] Job job)
        {
            if (ModelState.IsValid)
            {
                String create = "INSERT INTO jobs (Title) VALUES ('" + job.Title + "')";
                db.Create(create);
                //db.Courses.Add(course);
                //db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(job);
        }

        // GET: Jobs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            String view = "SELECT * FROM jobs WHERE JobID= '" + id + "'";

            var dt = db.List(view);
            var model = new Job().List(dt).FirstOrDefault();
            return View(model);
        }

        // POST: Jobs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "JobID,Title")] Job job)
        {
            if (ModelState.IsValid)
            {
                String edit = "UPDATE jobs SET Title = '" + job.Title + "' WHERE JobID = '" + job.JobID + "' ";
                db.Edit(edit);
                //db.Entry(course).State = EntityState.Modified;
                //db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(job);
        }

        // GET: Jobs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            String view = "SELECT * FROM jobs WHERE JobID= '" + id + "'";

            var dt = db.List(view);
            var model = new Job().List(dt).FirstOrDefault();
            return View(model);
        }

        // POST: Jobs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            String delete = "DELETE FROM jobs WHERE JobID = '" + id + "'";
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
