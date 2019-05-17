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
    public class StaffSchedulesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: StaffSchedules
        public ActionResult Index()
        {
            var staffSchedules = db.StaffSchedules.Include(s => s.course).Include(s => s.schedule).Include(s => s.staff);
            return View(staffSchedules.ToList());
        }

        // GET: StaffSchedules/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StaffSchedule staffSchedule = db.StaffSchedules.Find(id);
            if (staffSchedule == null)
            {
                return HttpNotFound();
            }
            return View(staffSchedule);
        }

        // GET: StaffSchedules/Create
        public ActionResult Create()
        {
            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "Title");
            ViewBag.ScheduleID = new SelectList(db.Schedules, "ScheduleID", "CourseID");
            ViewBag.StaffID = new SelectList(db.Staffs, "StaffID", "FirstName");
            return View();
        }

        // POST: StaffSchedules/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StaffID,CourseID,ScheduleID")] StaffSchedule staffSchedule)
        {
            if (ModelState.IsValid)
            {
                db.StaffSchedules.Add(staffSchedule);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "Title", staffSchedule.CourseID);
            ViewBag.ScheduleID = new SelectList(db.Schedules, "ScheduleID", "CourseID", staffSchedule.ScheduleID);
            ViewBag.StaffID = new SelectList(db.Staffs, "StaffID", "FirstName", staffSchedule.StaffID);
            return View(staffSchedule);
        }

        // GET: StaffSchedules/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StaffSchedule staffSchedule = db.StaffSchedules.Find(id);
            if (staffSchedule == null)
            {
                return HttpNotFound();
            }
            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "Title", staffSchedule.CourseID);
            ViewBag.ScheduleID = new SelectList(db.Schedules, "ScheduleID", "CourseID", staffSchedule.ScheduleID);
            ViewBag.StaffID = new SelectList(db.Staffs, "StaffID", "FirstName", staffSchedule.StaffID);
            return View(staffSchedule);
        }

        // POST: StaffSchedules/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StaffID,CourseID,ScheduleID")] StaffSchedule staffSchedule)
        {
            if (ModelState.IsValid)
            {
                db.Entry(staffSchedule).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "Title", staffSchedule.CourseID);
            ViewBag.ScheduleID = new SelectList(db.Schedules, "ScheduleID", "CourseID", staffSchedule.ScheduleID);
            ViewBag.StaffID = new SelectList(db.Staffs, "StaffID", "FirstName", staffSchedule.StaffID);
            return View(staffSchedule);
        }

        // GET: StaffSchedules/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StaffSchedule staffSchedule = db.StaffSchedules.Find(id);
            if (staffSchedule == null)
            {
                return HttpNotFound();
            }
            return View(staffSchedule);
        }

        // POST: StaffSchedules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            StaffSchedule staffSchedule = db.StaffSchedules.Find(id);
            db.StaffSchedules.Remove(staffSchedule);
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
