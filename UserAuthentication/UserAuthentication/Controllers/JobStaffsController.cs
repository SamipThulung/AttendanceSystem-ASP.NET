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
    public class JobStaffsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: JobStaffs
        public ActionResult Index()
        {
            String index = "SELECT * FROM jobstaffs";
            var dt = db.List(index);
            var model = new JobStaff().List(dt);
            return View(model);
            //var jobStaffs = db.JobStaffs.Include(j => j.job).Include(j => j.staff);
            //return View(jobStaffs.ToList());
        }

        // GET: JobStaffs/Create
        public ActionResult Create()
        {
            //get jobs
            String jobs = "SELECT * FROM jobs";
            var dt = db.List(jobs);
            var jModel = new Job().List(dt);
            //get staffs
            String index = "SELECT * FROM staffs";
            var dt2 = db.List(index);
            var sModel = new Staff().List(dt2);

           ViewBag.JobID = new SelectList(jModel, "JobID", "Title");
            ViewBag.StaffID = new SelectList(sModel, "StaffID", "FirstName");
            return View();
        }

        // POST: JobStaffs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StaffID,JobID")] JobStaff jobStaff)
        {
            if (ModelState.IsValid)
            {
                string insert = "Insert into JobStaffs (StaffID,JobID) VALUES ('" + jobStaff.StaffID + "'," + jobStaff.JobID + ")";
                db.Create(insert);
              
                return RedirectToAction("Index");
            }

              //get jobs
            String jobs = "SELECT * FROM jobs";
            var dt = db.List(jobs);
            var jModel = new Job().List(dt);
            //get staffs
            String index = "SELECT * FROM staffs";
            var dt2 = db.List(index);
            var sModel = new Staff().List(dt2);

            ViewBag.JobID = new SelectList(jModel, "JobID", "Title", jobStaff.JobID);
            ViewBag.StaffID = new SelectList(sModel, "StaffID", "FirstName", jobStaff.StaffID);
            return View(jobStaff);
        }
        
        // GET: JobStaffs/Delete/5
        public ActionResult Delete(string s_id,int j_id)
        {
            String view = "SELECT * FROM scheduleGroups WHERE StaffID= '" + s_id + "'  AND JobID = " + j_id + "   ";
            var dt = db.List(view);
            var model = new ScheduleGroup().List(dt).FirstOrDefault();

            return View(model);
        }

        // POST: JobStaffs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string s_id, int j_id)
        {
            String delete = "DELETE FROM JobStaffs WHERE JobID = " + j_id + " AND StaffID = '" + s_id + "' ";
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
