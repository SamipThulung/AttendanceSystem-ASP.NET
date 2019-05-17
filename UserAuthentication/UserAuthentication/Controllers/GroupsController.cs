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
    public class GroupsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Groups
        public ActionResult Index()
        {
            String sql = "SELECT * FROM groups INNER JOIN faculties ON groups.facultyid = faculties.facultyid";
            db.List(sql);
            var dt = db.List(sql);
            var model = new Group().List(dt);
            return View(model);
        }

        // GET: Groups/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            String sql = "SELECT * FROM groups INNER JOIN faculties ON groups.facultyid = faculties.facultyid  WHERE GroupID= '" + id + "'";
            db.List(sql);
            var dt = db.List(sql);
            var model = new Group().List(dt).FirstOrDefault();
            return View(model);
        }

        // GET: Groups/Create
        public ActionResult Create()
        {
            string sql = "Select * From Faculties;";
            var a = db.List(sql);
            var FacultiesList = new Faculty().List(a); 
            ViewBag.FacultyID = new SelectList(FacultiesList, "FacultyID", "Name");
            return View();
        }

        // POST: Groups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "GroupID,Semester,Year,FacultyID")] Group group)
        {
            if (ModelState.IsValid)
            {
                string sql = "Insert Into Groups(GroupID,Semester,Year,FacultyID) Values('" + group.GroupID + "','" + group.Semester + "','" + group.Year + "','" + group.FacultyID + "')";
                db.Create(sql);
                // db.Groups.Add(group);
                //db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.FacultyID = new SelectList(db.Faculties, "FacultyID", "Name", group.FacultyID);
            return View(group);
        }

        // GET: Groups/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group group = db.Groups.Find(id);
            if (group == null)
            {
                return HttpNotFound();
            }
            ViewBag.FacultyID = new SelectList(db.Faculties, "FacultyID", "Name", group.FacultyID);
            return View(group);
        }

        // POST: Groups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "GroupID,Semester,Year,FacultyID")] Group group)
        {
            if (ModelState.IsValid)
            {
                 string sql = "Update Groups SET GroupID ='" + group.GroupID + "',Semester='" + group.Semester + "',Year='" + group.Year + "',FacultyID='" + group.FacultyID + "' Where GroupID='" + group.GroupID+"'";
                db.Edit(sql);
               // db.Entry(group).State = EntityState.Modified;
                //db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.FacultyID = new SelectList(db.Faculties, "FacultyID", "Name", group.FacultyID);
            return View(group);
        }

        // GET: Groups/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            String sql = "SELECT * FROM groups where GroupID= '" + id + "'";
            db.List(sql);
            var dt = db.List(sql);
            var model = new Group().List(dt).FirstOrDefault();
            return View(model);
        }

        // POST: Groups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            string sql = "DELETE FROM groups WHERE GroupID= '" + id + "'";
            db.Delete(sql);
            //Group group = db.Groups.Find(id);
            //db.Groups.Remove(group);
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
