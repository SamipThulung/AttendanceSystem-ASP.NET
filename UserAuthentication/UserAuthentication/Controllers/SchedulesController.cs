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
    [Authorize(Roles = "Admin,Student Service")]
    public class SchedulesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Schedules
        public ActionResult Index()
        {
            String view = "SELECT sch.scheduleID,sch.CourseID,sch.Classroom,sch.ClassType,sch.Day,sch.StartTime,sch.EndTime,c.Title AS  CourseTitle FROM schedules sch JOIN Courses c ON c.CourseID = sch.CourseID";
            db.List(view);
            var dt = db.List(view);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Debug.WriteLine(dt.Rows[i]["CourseTitle"]);
            }
            var model = new Schedule().List(dt);
            return View(model);
        }

        // GET: Schedules/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            String view = "SELECT * FROM schedules WHERE ScheduleID= '" + id + "'";
            var dt = db.List(view);
            var model = new Schedule().List(dt).FirstOrDefault();
            return View(model);
        }

        // GET: Schedules/Create
        public ActionResult Create()
        {
            String view = "SELECT * FROM courses";
            db.List(view);
            var dt = db.List(view);
            var model = new Course().List(dt);
            ViewBag.CourseID = new SelectList(model, "CourseID", "Title");
            return View();
        }

        // POST: Schedules/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ScheduleID,CourseID,Classroom,ClassType,Day,StartTime,EndTime")] Schedule schedule)
        {
            if (ModelState.IsValid)
            {
                String create = "INSERT INTO schedules (CourseID,Classroom,ClassType,Day,StartTime,EndTime) VALUES ('" + schedule.CourseID + "' , '" + schedule.Classroom + "' , '" + schedule.ClassType + "' , '" + schedule.Day + "', '"+schedule.StartTime+"' , '"+schedule.EndTime+"')";
                db.Create(create);
                return RedirectToAction("Index");
            }
            String view = "SELECT * FROM courses";
            db.List(view);
            var dt = db.List(view);
            var model = new Course().List(dt);
            ViewBag.CourseID = new SelectList(model, "CourseID", "Title", schedule.CourseID);
            return View(schedule);
        }

        // GET: Schedules/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Schedule schedule = db.Schedules.Find(id);
            if (schedule == null)
            {
                return HttpNotFound();
            }
            String view = "SELECT * FROM courses";
            db.List(view);
            var dt = db.List(view);
            var model = new Course().List(dt);
            ViewBag.CourseID = new SelectList(model, "CourseID", "Title", schedule.CourseID);
            return View(schedule);
        }

        // POST: Schedules/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ScheduleID,CourseID,Classroom,ClassType,Day,StartTime,EndTime")] Schedule schedule)
        {
            if (ModelState.IsValid)
            {
                String edit = "UPDATE schedules SET Classroom = '" + schedule.Classroom + "' , ClassType = '" + schedule.ClassType + "' , Day = '" + schedule.Day + "' , StartTime = '" + schedule.StartTime + "', EndTime = '" + schedule.EndTime + "' WHERE ScheduleID = '" + schedule.ScheduleID + "' ";
                db.Edit(edit);
                return RedirectToAction("Index");
            }
            String view = "SELECT * FROM courses";
            db.List(view);
            var dt = db.List(view);
            var model = new Course().List(dt);
            ViewBag.CourseID = new SelectList(model, "CourseID", "Title", schedule.CourseID);
            return View(schedule);
        }

        // GET: Schedules/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Schedule schedule = db.Schedules.Find(id);
            if (schedule == null)
            {
                return HttpNotFound();
            }
            return View(schedule);
        }

        // POST: Schedules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            String delete = "DELETE FROM schedules WHERE ScheduleID = '" + id + "'";
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
