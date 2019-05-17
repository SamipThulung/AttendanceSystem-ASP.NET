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
    public class DepartmentStaffsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: DepartmentStaffs
        public ActionResult Index()
        {
            String index = "SELECT * FROM departmentstaffs";
            var dt = db.List(index);
            var model = new DepartmentStaff().List(dt);
            return View(model);
            //var departmentStaffs = db.DepartmentStaffs.Include(d => d.departments).Include(d => d.staff);
            //return View(departmentStaffs.ToList());
        }

        // GET: DepartmentStaffs/Create
        public ActionResult Create()
        {
            //get departments
            String department = "SELECT * FROM departments";
            var dt = db.List(department);
            var dModel = new Departments().List(dt);
            //get Staffs
            String staff = "SELECT * FROM staffs";
            var dt2 = db.List(staff);
            var sModel = new Staff().List(dt2);

            ViewBag.DepartmentID = new SelectList(dModel, "DepartmentID", "DepartmentName");
            ViewBag.StaffID = new SelectList(sModel, "StaffID", "FirstName");
            return View();
        }

        // POST: DepartmentStaffs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DepartmentID,StaffID")] DepartmentStaff departmentStaff)
        {
            if (ModelState.IsValid)
            {
                string swl = "Insert into DepartmentStaffs (DepartmentID,StaffID) VALUES (" + departmentStaff.DepartmentID + ",'" + departmentStaff.StaffID + "')";
                db.Create(swl);
                return RedirectToAction("Index");
            }
            //get departments
            String department = "SELECT * FROM departments";
            var dt = db.List(department);
            var dModel = new Departments().List(dt);
            //get Staffs
            String staff = "SELECT * FROM staffs";
            var dt2 = db.List(staff);
            var sModel = new Departments().List(dt2);

            ViewBag.DepartmentID = new SelectList(dModel, "DepartmentID", "DepartmentName", departmentStaff.DepartmentID);
            ViewBag.StaffID = new SelectList(sModel, "StaffID", "FirstName", departmentStaff.StaffID);
            return View(departmentStaff);
        }
        
        // GET: DepartmentStaffs/Delete/5
        public ActionResult Delete(string d_id,string s_id)
        {
           
            string view = "SELECT * FROM DepartmentStaffs WHERE StaffID='" + s_id + "'  AND DepartmentID = " + d_id + "   ";
            var dt = db.List(view);
            var model = new DepartmentStaff().List(dt).FirstOrDefault();

            return View(model);
        }

        // POST: DepartmentStaffs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string d_id, string s_id)
        {
            String delete = "DELETE FROM DepartmentStaffs WHERE DepartmentID = " + d_id + " AND StaffID = '" + s_id + "' ";
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
