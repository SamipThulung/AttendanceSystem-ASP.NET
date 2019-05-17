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
    [Authorize(Roles = "Admin,Student Service,Teacher,Lecturer,Tutor")]
    public class AttendancesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Attendances
        public ActionResult Index()
        {
            if (TempData["sch_id"] == null)
            {
                //string sql = "Select s.StudentID, s.FirstName,s.LastName, a.Status from Attendances a JOIN Students s ON s.StudentID = a.StudentID;";
                //DataTable dt = db.List(sql);

                //Debug.WriteLine("Count==================> : " + dt.Rows.Count);
                //List<Attendance> list = new List<Attendance>();
                //Debug.WriteLine("Rows==================> : " + dt.Rows);
                //for (int i = 0; i < dt.Rows.Count; i++)
                //{
                //    Attendance ad = new Attendance();
                //    ad.StudentID = dt.Rows[i]["StudentID"].ToString();
                //    ad.FirstName = dt.Rows[i]["FirstName"].ToString();
                //    ad.LastName = dt.Rows[i]["LastName"].ToString();
                //    ad.Status = dt.Rows[i]["Status"].ToString();
                //    list.Add(ad);


                //}
                List<Attendance> list = new List<Attendance>();
                string sql = "Select sh.ScheduleID, c.Title, sh.Classroom, sh.ClassType from Schedules sh JOIN Courses c ON sh.CourseID = c.CourseID;";
           
                

 
                DataTable dt = db.List(sql);

                List<Schedule> scheduleList = new List<Schedule>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Schedule ad = new Schedule();
                    ad.ScheduleID = Convert.ToInt16(dt.Rows[i]["ScheduleID"]);
                    ad.Title = dt.Rows[i]["Title"].ToString();
                    ad.Classroom = dt.Rows[i]["Classroom"].ToString();
                    ad.ClassType = dt.Rows[i]["ClassType"].ToString();
                  
                   
                    scheduleList.Add(ad);
                }

                ViewBag.ScheduleID = new SelectList(scheduleList, "ScheduleID", "FullSchedule");


                return View(list);
            }
            else
            {
                try
                {
                    string sql = "Select s.StudentID, s.FirstName,s.LastName, a.Status from Attendances a JOIN Students s ON s.StudentID = a.StudentID where a.ScheduleID= " + TempData["sch_id"] + " and a.AttendanceDate = '"+TempData["AttendanceDate"] +"';";
                    DataTable dt = db.List(sql);

                    Debug.WriteLine("Count when Date is Given==================> : " + dt.Rows.Count);
                    List<Attendance> list = new List<Attendance>();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Attendance ad = new Attendance();
                        ad.StudentID = dt.Rows[i]["StudentID"].ToString();
                        ad.FirstName = dt.Rows[i]["FirstName"].ToString();
                        ad.LastName = dt.Rows[i]["LastName"].ToString();
                        ad.Status = dt.Rows[i]["Status"].ToString();
                        list.Add(ad);
                    }

                    ViewBag.ScheduleID = new SelectList(db.Schedules, "ScheduleID", "FullSchedule");
                    return View(list);

                }
                catch (Exception e )
                {
      
                    List<Attendance> list = new List<Attendance>();
                    string sql = "Select sh.ScheduleID, c.Title, sh.Classroom, sh.ClassType from Schedules sh JOIN Courses c ON sh.CourseID = c.CourseID;";


             

                    DataTable dt = db.List(sql);

                    List<Schedule> scheduleList = new List<Schedule>();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Schedule ad = new Schedule();
                        ad.ScheduleID = Convert.ToInt16(dt.Rows[i]["ScheduleID"]);
                        ad.Title = dt.Rows[i]["Title"].ToString();
                        ad.Classroom = dt.Rows[i]["Classroom"].ToString();
                        ad.ClassType = dt.Rows[i]["ClassType"].ToString();


                        scheduleList.Add(ad);
                    }

                    ViewBag.ScheduleID = new SelectList(scheduleList, "ScheduleID", "FullSchedule");


                    return View(list);
                 
                }
               
            }
           
        }
       
        [HttpPost]
        public ActionResult Index(FormCollection fm, string submitButton)
        {
            Debug.WriteLine("Sumit==================> : " + submitButton);
            if (submitButton=="Record")
            {
                TempData["ScheduleID"] = Convert.ToInt16(fm["ScheduleID"]);
                int schID = Convert.ToInt16(fm["ScheduleID"]);

                Debug.WriteLine("ScheduleID : " + schID);
                string sql = "Select s.StudentID, s.FirstName,s.LastName From Students s JOIN GROUPS g ON g.GroupID = s.GroupID JOIN ScheduleGroups sg ON sg.GroupID = g.GroupID JOIN Schedules sch ON sch.ScheduleID = sg.ScheduleID Where sch.ScheduleID = " + schID + ";";
                DataTable dt = db.List(sql);

                Debug.WriteLine("Count==================> : " + dt.Rows.Count);
                List<Student> list = new List<Student>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Student std = new Student();
                    std.StudentID = dt.Rows[i]["StudentID"].ToString();
                    std.FirstName = dt.Rows[i]["FirstName"].ToString();
                    std.LastName = dt.Rows[i]["LastName"].ToString();
                    list.Add(std);
                }

                Debug.WriteLine("Preparing List");
                Debug.WriteLine(list[0].FirstName);
                TempData["list"] = list;
                return RedirectToAction("AttendanceStudent");
            }
            else
            {
                Debug.WriteLine(fm["ScheduleID"]);
                TempData["sch_id"] = fm["ScheduleID"];
                TempData["AttendanceDate"] = fm["AttendanceDate"];
                return RedirectToAction("Index");
            }
            
        }

        // GET: Attendances/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Attendance attendance = db.Attendances.Find(id);
            if (attendance == null)
            {
                return HttpNotFound();
            }
            return View(attendance);
        }

        // GET: Attendances/Create
        public ActionResult Create()
        {
            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "CourseID");
            ViewBag.ScheduleID = new SelectList(db.Schedules, "ScheduleID", "ScheduleID");
            ViewBag.StudentID = new SelectList(db.Students, "StudentID", "StudentID");

            var attendances = db.Attendances.Include(a => a.course).Include(a => a.schedule).Include(a => a.std);
            return View(attendances.ToList());
        }

        // POST: Attendances/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(List<Attendance> attendance)
        {
            if (ModelState.IsValid)
            {
                foreach (var item in attendance)
                {
                    db.Entry(item).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return RedirectToAction("Index");
            }

            //ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "Title", attendance.CourseID);
            //ViewBag.ScheduleID = new SelectList(db.Schedules, "ScheduleID", "CourseID", attendance.ScheduleID);
            //ViewBag.StudentID = new SelectList(db.Students, "StudentID", "FirstName", attendance.StudentID);
            //return View(attendance);
            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "CourseID");
            ViewBag.ScheduleID = new SelectList(db.Schedules, "ScheduleID", "ScheduleID");
            ViewBag.StudentID = new SelectList(db.Students, "StudentID", "StudentID");

            var attendances = db.Attendances.Include(a => a.course).Include(a => a.schedule).Include(a => a.std);
            return View(attendances.ToList());
        }

        // GET: Attendances/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Attendance attendance = db.Attendances.Find(id);
            if (attendance == null)
            {
                return HttpNotFound();
            }
            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "Title", attendance.CourseID);
            ViewBag.ScheduleID = new SelectList(db.Schedules, "ScheduleID", "CourseID", attendance.ScheduleID);
            ViewBag.StudentID = new SelectList(db.Students, "StudentID", "FirstName", attendance.StudentID);
            return View(attendance);
        }

        // POST: Attendances/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AttendanceID,ScheduleID,CourseID,Status,Record,AttendanceDate,StudentID")] Attendance attendance)
        {
            if (ModelState.IsValid)
            {
                db.Entry(attendance).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "Title", attendance.CourseID);
            ViewBag.ScheduleID = new SelectList(db.Schedules, "ScheduleID", "CourseID", attendance.ScheduleID);
            ViewBag.StudentID = new SelectList(db.Students, "StudentID", "FirstName", attendance.StudentID);
            return View(attendance);
        }

        // GET: Attendances/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Attendance attendance = db.Attendances.Find(id);
            if (attendance == null)
            {
                return HttpNotFound();
            }
            return View(attendance);
        }

        // POST: Attendances/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Attendance attendance = db.Attendances.Find(id);
            db.Attendances.Remove(attendance);
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

        public ActionResult AttendanceStudent()
        {
          
       
          var list = TempData["list"] as List<Student>;
          ViewBag.Model = list;

          return View(list);
              
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AttendanceStudent(FormCollection student)
        {
            
            string scheduleID = TempData["ScheduleID"].ToString();

            string sqlcourse = "SELECT CourseID from Schedules where ScheduleID=" + scheduleID + ";";

            DateTime date = Convert.ToDateTime(student["AttendanceDate"]);
            DataTable course = db.List(sqlcourse);
            DataRow aaa = course.Rows[0];
            string courseID = aaa["CourseID"].ToString();
            System.Diagnostics.Debug.WriteLine("Entered");
            System.Diagnostics.Debug.WriteLine(student);
            Debug.WriteLine(date);
            try
            {
                int count = 0;
                foreach (var item in student)
                {
                    

                    if (count>1)
                    {
                        System.Diagnostics.Debug.WriteLine("This is");
                        System.Diagnostics.Debug.WriteLine(item);
                        System.Diagnostics.Debug.WriteLine(student[item.ToString()]);
                        string sql = "Insert into Attendances (ScheduleID, CourseID, Status, AttendanceDate, StudentID) Values  ( " + scheduleID + ",'" + courseID + "','" + student[item.ToString()] + "', '" + student["AttendanceDate"] + "','" + item + "'); ";
                        db.Create(sql);
                        Debug.WriteLine("ObjectType" + item.GetType());
                    }

                    count += 1;
                   
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                
            }
            

            return RedirectToAction("Index");
        }
    }
}
