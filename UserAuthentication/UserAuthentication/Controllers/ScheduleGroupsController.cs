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
    public class ScheduleGroupsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ScheduleGroups
        public ActionResult Index()
        {
            String index = "Select * FROM schedulegroups";
            var dt = db.List(index);
            var model = new ScheduleGroup().List(dt);
            return View(model);
            //var scheduleGroups = db.ScheduleGroups.Include(s => s.group).Include(s => s.schedule);
            //return View(scheduleGroups.ToList());
        }


        // GET: ScheduleGroups/Create
        public ActionResult Create()
        {
            //get groups
            String index = "Select * FROM groups g JOIN Faculties f ON g.FacultyID = f.FacultyID  ";
            var dt = db.List(index);
            var model = new Group().List(dt);
            //get schedules
            String index2 = "Select * FROM schedules";
            DataTable dt2 = db.List(index2);
            var model2 = new Schedule().List(dt2);

            ViewBag.GroupID = new SelectList(model, "GroupID", "GroupID");
            ViewBag.ScheduleID = new SelectList(model2, "ScheduleID", "ScheduleID");
            return View();
        }

        // POST: ScheduleGroups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ScheduleID,GroupID")] ScheduleGroup scheduleGroup)
        {
            if (ModelState.IsValid)
            {
                string insert = "Insert into ScheduleGroups (ScheduleID,GroupID) VALUES (" + scheduleGroup.ScheduleID + ",'" + scheduleGroup.GroupID + "')";
                db.Create(insert);
                return RedirectToAction("Index");
            }
            //get groups
            String index = "Select * FROM groups";
            var dt = db.List(index);
            var model = new Group().List(dt);
            //get schedules
            String index2 = "Select * FROM schedules";
            var dt2 = db.List(index2);
            var model2 = new Schedule().List(dt2);

            ViewBag.GroupID = new SelectList(model, "GroupID", "Semester", scheduleGroup.GroupID);
            ViewBag.ScheduleID = new SelectList(model2, "ScheduleID", "CourseID", scheduleGroup.ScheduleID);
            return View(scheduleGroup);
        }

      
        // GET: ScheduleGroups/Delete/5
        public ActionResult Delete(string sch_id, string g_id)
        {
            if (sch_id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            String view = "SELECT * FROM scheduleGroups WHERE ScheduleID= '" + sch_id + "'  AND GroupID = '"+g_id+"'   ";
            var dt = db.List(view);
            var model = new ScheduleGroup().List(dt).FirstOrDefault();
            return View(model);

        }

        // POST: ScheduleGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string sch_id, string g_id )
        {
            String delete = "DELETE FROM schedulegroups WHERE ScheduleID = '" + sch_id + "' AND GroupID = '" + g_id + "'  ";
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