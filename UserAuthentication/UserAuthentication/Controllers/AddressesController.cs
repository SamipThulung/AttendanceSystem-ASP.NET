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
    public class AddressesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Addresses
        public ActionResult Index()
        {
            String sql = "SELECT * FROM addresses ";
            db.List(sql);
            var dt = db.List(sql);
            var model = new Address().List(dt);
            return View(model);
        }

        // GET: Addresses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            String sql = "SELECT * FROM addresses WHERE AddressID= '" + id + "'";
            db.List(sql);
            var dt = db.List(sql);
            var model = new Address().List(dt).FirstOrDefault();
            return View(model);
        }

        // GET: Addresses/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Addresses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AddressID,AddressName")] Address address)
        {
            if (ModelState.IsValid)
            {
                string sql = "Insert Into Addresses(AddressName) Values('" + address.AddressName + "')";
                db.Create(sql);
                //db.Addresses.Add(address);
                //db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(address);
        }

        // GET: Addresses/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Address address = db.Addresses.Find(id);
            if (address == null)
            {
                return HttpNotFound();
            }
            return View(address);
        }

        // POST: Addresses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AddressID,AddressName")] Address address)
        {
            if (ModelState.IsValid)
            {
                string sql = "Update Faculties SET AddressName ='" + address.AddressName + "'";
                db.Edit(sql);
                //db.Entry(address).State = EntityState.Modified;
                //db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(address);
        }

        // GET: Addresses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            String sql = "SELECT * FROM addresses where AddressID= '" + id + "'";
            db.List(sql);
            var dt = db.List(sql);
            var model = new Address().List(dt).FirstOrDefault();
            return View(model);
        }

        // POST: Addresses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            string sql = "DELETE FROM addresses WHERE AddressID= '" + id + "'";
            db.Delete(sql);
            //Address address = db.Addresses.Find(id);
            //db.Addresses.Remove(address);
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
