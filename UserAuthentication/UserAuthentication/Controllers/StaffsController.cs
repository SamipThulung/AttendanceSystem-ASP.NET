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
    public class StaffsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Staffs
        public ActionResult Index()
        {
            String view = "SELECT s.StaffID,s.FirstName,s.LastName,s.ContactNo,s.DOB,j.Title JobTitle FROM staffs s JOIN Jobstaffs js ON js.StaffID = s.StaffID JOIN Jobs j ON j.JobID=js.JobID";
            db.List(view);
            var dt = db.List(view);
            var model = new Staff().List(dt);
            return View(model);
        }

        // GET: Staffs/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            String view = "SELECT * FROM staffs WHERE StaffID= '" + id + "'";

            var dt = db.List(view);
            var model = new Staff().List(dt).FirstOrDefault();
            return View(model);
        }

        [Authorize(Roles = "Admin,Student Service")]
        // GET: Staffs/Create
        public ActionResult Create()
        {
            String index = "SELECT * FROM Jobs";
            var dt2 = db.List(index);
            var sModel = new Job().List(dt2);

            ViewBag.JobID = new SelectList(sModel, "JobID", "Title");
            return View();
        }

        // POST: Staffs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StaffID,FirstName,LastName,ContactNo,DOB,JobID")] Staff staff)
        {
            
                String create = "INSERT INTO staffs (StaffID,FirstName,LastName,ContactNo,DOB,JobID) VALUES ('" + staff.StaffID + "' , '" + staff.FirstName + "' , '" + staff.LastName + "' , '" + staff.ContactNo + "' , '" + staff.DOB + "',"+staff.JobID+")";
                db.Create(create);
            //db.Courses.Add(course);
            //db.SaveChanges();
            string creat = "Insert into Jobstaffs (JobID,StaffID) VALUES ('"+staff.JobID+"','"+staff.StaffID+"') ";
            db.Create(creat);
                return RedirectToAction("Index");
            

            
        }

        // GET: Staffs/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            String view = "SELECT * FROM staffs WHERE StaffID= '" + id + "'";

            var dt = db.List(view);
            var model = new Staff().List(dt).FirstOrDefault();
            return View(model);
        }

        // POST: Staffs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StaffID,FirstName,LastName,ContactNo,DOB")] Staff staff)
        {
            if (ModelState.IsValid)
            {
                String edit = "UPDATE staffs SET FirstName = '" + staff.FirstName + "' , LastName = '" + staff.LastName + "' , ContactNo = '" + staff.ContactNo + "', DOB = '" + staff.DOB + "' WHERE StaffID = '" + staff.StaffID + "' ";
                db.Edit(edit);
                //db.Entry(course).State = EntityState.Modified;
                //db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(staff);
        }

        // GET: Staffs/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            String view = "SELECT * FROM staffs WHERE StaffID= '" + id + "'";

            var dt = db.List(view);
            var model = new Staff().List(dt).FirstOrDefault();
            return View(model);
        }

        // POST: Staffs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            String delete = "DELETE FROM staffs WHERE StaffID = '" + id + "'";
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