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
    public class InstitutesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Institutes
        public ActionResult Index()
        {
            String sql = "SELECT * FROM institutes ";
            db.List(sql);
            var dt = db.List(sql);
            var model = new Institute().List(dt);
            return View(model);
        }

        // GET: Institutes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            String sql = "SELECT * FROM institutes WHERE InstituteID= '" + id + "'";
            db.List(sql);
            var dt = db.List(sql);
            var model = new Institute().List(dt).FirstOrDefault();
            return View(model);
        }

        // GET: Institutes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Institutes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "InstituteID,Name")] Institute institute)
        {
            if (ModelState.IsValid)
            {
                string sql = "Insert Into Institutes(Name) Values('" + institute.Name + "')";
                db.Create(sql);
                //db.Institutes.Add(institute);
                //db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(institute);
        }

        // GET: Institutes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Institute institute = db.Institutes.Find(id);
            if (institute == null)
            {
                return HttpNotFound();
            }
            return View(institute);
        }

        // POST: Institutes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "InstituteID,Name")] Institute institute)
        {
            if (ModelState.IsValid)
            {
                string sql = "Update Institutes SET Name ='" + institute.Name + "'";
                db.Edit(sql);
                //db.Entry(institute).State = EntityState.Modified;
                //db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(institute);
        }

        // GET: Institutes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            String sql = "SELECT * FROM institutes where InstituteID= '" + id + "'";
            db.List(sql);
            var dt = db.List(sql);
            var model = new Address().List(dt).FirstOrDefault();
            return View(model);
        }

        // POST: Institutes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            string sql = "DELETE FROM institutes WHERE InstituteID= '" + id + "'";
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
