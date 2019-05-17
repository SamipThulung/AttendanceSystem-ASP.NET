using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UserAuthentication.Models;

namespace UserAuthentication.Controllers
{
    [Authorize(Roles = "Admin,Student Service")]
    public class ReportController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Report
        public ActionResult Index()
        {
            string studentsql = "Select * from Students s JOIN Groups g ON g.GroupID = s.GroupID Order BY s.EnrollDate ASC";
            DataTable a = db.List(studentsql);

            string courses = "Select * from courses";
            DataTable ab = db.List(courses);
            var CourseID = new Course().List(ab);

            ViewBag.CourseID = new SelectList(CourseID, "CourseID", "Title");
            var list = new Student().List(a);
            return View(list);
        }

        [HttpPost]
         public ActionResult Index(FormCollection fm)
        {
            
            string studentsql = "Select * from Students s JOIN Groups g ON g.GroupID = s.GroupID JOIN CourseStudents cs ON s.StudentID = cs.StudentID Where cs.CourseID = '"+fm["CourseID"]+"' Order BY s.EnrollDate ASC";
            DataTable a = db.List(studentsql);

            string courses = "Select * from courses";
            DataTable ab = db.List(courses);
            var CourseID = new Course().List(ab);

            ViewBag.CourseID = new SelectList(CourseID, "CourseID", "Title");
            var list = new Student().List(a);
            return View(list);
        }

        public ActionResult IndividualStudentReport(string id)
        {
            Debug.WriteLine(id);
            DateTime date = DateTime.Now.Date;
            string sql = "Select s.FirstName, s.LastName, c.Title, a.CourseID,a.Status,a.AttendanceDate from Attendances a JOIN Students s ON a.StudentID = s.StudentID JOIN Courses c ON c.CourseID = a.CourseID Where s.StudentID='" + id + "' and a.AttendanceDate='" + date + "'";


            DataTable dt = db.List(sql);
            if (dt.Rows.Count > 0)
            {
                var list = new Attendance().List(dt).FirstOrDefault();
                return View(list);
            }
            else
            {
                var list = new Attendance();
                return View(list);
              
            }
           
            
        }
        public ActionResult IndividualStudentWeekly(string id)
        {
            DateTime ss = DateTime.Now.Date;
            Debug.WriteLine("This si averfdsa  ");
            

            DateTime startDay = FirstDayOfWeek(ss);
            DateTime endDay = startDay.AddDays(7);

            string sql = "Select s.FirstName, s.LastName, c.Title, a.CourseID,a.Status,a.AttendanceDate from Attendances a JOIN Students s ON a.StudentID = s.StudentID JOIN Courses c ON c.CourseID = a.CourseID Where s.StudentID='" + id + "' and a.AttendanceDate Between '" + startDay + "' and '"+endDay+"'";
            DataTable table = db.List(sql);
            Debug.WriteLine(table.Rows.Count);
            var list = new Attendance().List(table);
            //Debug.WriteLine(list.FirstOrDefault().AttendanceDate);
            return View(list);

        }

        [HttpPost]
        public ActionResult IndividualStudentWeekly(FormCollection fm, string id)
        {
            DateTime ss = Convert.ToDateTime(fm["DatePicker11"]);
            DateTime startDay = FirstDayOfWeek(ss);
            DateTime endDay = startDay.AddDays(7);

            string sql = "Select s.FirstName, s.LastName, c.Title, a.CourseID,a.Status,a.AttendanceDate from Attendances a JOIN Students s ON a.StudentID = s.StudentID JOIN Courses c ON c.CourseID = a.CourseID Where s.StudentID='" + id+ "' and a.AttendanceDate Between '" + startDay + "' and '" + endDay + "'";
            DataTable table = db.List(sql);
            if (table.Rows.Count > 0)
            {
                var list = new Attendance().List(table);
                return View(list);
            }
            else
            {
                var list = new List<Attendance>();
                return View(list);

            }
        }
        public static DateTime FirstDayOfWeek(DateTime dt)
        {
            var culture = System.Threading.Thread.CurrentThread.CurrentCulture;
            var diff = dt.DayOfWeek - culture.DateTimeFormat.FirstDayOfWeek;
            if (diff < 0)
                diff += 7;
            return dt.AddDays(-diff).Date;
        }

        public ActionResult IndividualStudentMonthly(string id)
        {
            DateTime now = DateTime.Now;
            DateTime startDate = new DateTime(now.Year, now.Month, 1);
            DateTime endDate = startDate.AddMonths(1).AddDays(-1);

            string sql = "Select s.FirstName, s.LastName, c.Title, a.CourseID,a.Status,a.AttendanceDate from Attendances a JOIN Students s ON a.StudentID = s.StudentID JOIN Courses c ON c.CourseID = a.CourseID Where s.StudentID='" + id + "' and a.AttendanceDate Between '" + startDate + "' and '" + endDate + "'";
            DataTable dt = db.List(sql);
            if (dt.Rows.Count > 0)
            {
                var list = new Attendance().List(dt);
                return View(list);
            }
            else
            {
                var list = new List<Attendance>();
                return View(list);

            }

        }
        [HttpPost]
        public ActionResult IndividualStudentMonthly(string id,FormCollection fm)
        {
            DateTime now = Convert.ToDateTime(fm["DatePicker11"]);
            DateTime startDate = new DateTime(now.Year, now.Month, 1);
            DateTime endDate = startDate.AddMonths(1).AddDays(-1);

            string sql = "Select s.FirstName, s.LastName, c.Title, a.CourseID,a.Status,a.AttendanceDate from Attendances a JOIN Students s ON a.StudentID = s.StudentID JOIN Courses c ON c.CourseID = a.CourseID Where s.StudentID='" + id + "' and a.AttendanceDate Between '" + startDate + "' and '" + endDate + "'";
            DataTable dt = db.List(sql);
            if (dt.Rows.Count > 0)
            {
                var list = new Attendance().List(dt);
                return View(list);
            }
            else
            {
                var list = new List<Attendance>();
                return View(list);

            }

        }

        public ActionResult Summary(string id)
        {
            string sql = "Select * from Courses";
            DataTable dt = db.List(sql);
            List<Course> courselist = new List<Course>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Course ad = new Course();
                ad.CourseID = dt.Rows[i]["CourseID"].ToString();
                ad.Title = dt.Rows[i]["Title"].ToString();
                ad.CreditHour = Convert.ToInt16(dt.Rows[i]["CreditHour"]);
                ad.FacultyID = dt.Rows[i]["FacultyID"].ToString();


                courselist.Add(ad);
            }

            ViewBag.CourseList = new SelectList(courselist, "CourseID", "Title");
   
            List<Attendance> list = new List<Attendance>();
            ViewBag.WeeklyReport = list;
            ViewBag.MonthlyReport = list;
            return View(list);
        }

        [HttpPost]
        public ActionResult Summary(string id, FormCollection fm)
        {
            string sql = "Select * from Courses";
            DataTable dt = db.List(sql);
            List<Course> courselist = new List<Course>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Course ad = new Course();
                ad.CourseID = dt.Rows[i]["CourseID"].ToString();
                ad.Title = dt.Rows[i]["Title"].ToString();
                ad.CreditHour = Convert.ToInt16(dt.Rows[i]["CreditHour"]);
                ad.FacultyID = dt.Rows[i]["FacultyID"].ToString();


                courselist.Add(ad);
            }

            ViewBag.CourseList = new SelectList(courselist, "CourseID", "Title");

            DateTime now = Convert.ToDateTime(fm["DatePicker11"]);
            DateTime startDateMonth = new DateTime(now.Year, now.Month, 1);
            DateTime endDateMonth = startDateMonth.AddMonths(1).AddDays(-1);

        
            DateTime startDateWeek = FirstDayOfWeek(now);
            DateTime endDateWeek = startDateWeek.AddDays(7);

            List<Attendance> att = new List<Attendance>();

            
            string getAtttWeek = "Select c.Title, Count(c.CourseID) as Present,a.Status  from Attendances a JOIN Courses c ON c.CourseID = a.CourseID Where a.StudentID='" + id+"' and a.AttendanceDate Between '"+startDateWeek+"' and '"+endDateWeek+"' Group By c.Title,a.Status Order By c.Title";
            DataTable tt = db.List(getAtttWeek);

            var weeklyReport = new Attendance().ListToCount(tt);

            Debug.WriteLine("Post======>");
           

            string getAttMonthly = "Select c.Title, Count(c.CourseID)  as Present,a.Status from Attendances a JOIN Courses c ON c.CourseID = a.CourseID Where a.StudentID='" + id + "' and a.AttendanceDate Between '" + startDateMonth + "' and '" + endDateMonth + "' Group By c.Title,a.Status Order By c.Title ";
            DataTable hh = db.List(getAttMonthly);
            var monthlyReport = new Attendance().ListToCount(hh);
            List<Attendance> list = new List<Attendance>();

            if (hh.Rows.Count > 0 && tt.Rows.Count == 0)
            {

                ViewBag.WeeklyReport = list;
                ViewBag.MonthlyReport = monthlyReport;
            

            }else if (hh.Rows.Count == 0 && tt.Rows.Count > 0)
            {
                ViewBag.WeeklyReport =  monthlyReport;
                ViewBag.MonthlyReport = list;
            }
            else if (hh.Rows.Count > 0 && tt.Rows.Count > 0)
            {
                ViewBag.WeeklyReport = weeklyReport;
                ViewBag.MonthlyReport = monthlyReport;
            }
            else
            {
              
                ViewBag.WeeklyReport = list;
                ViewBag.MonthlyReport = list;

            }
            return View();

        }
        public ActionResult TotalWorkWeekHR()
        {
            string sql = "Select s.StaffID, s.FirstName, s.LastName,s.ContactNo,s.DOB, j.Title as JobTitle from Staffs s JOIN JobStaffs js ON js.StaffID = s.StaffID JOIN Jobs j ON j.JobID = js.JobID Where j.Title IN ('Lecturer','Tutor')";

            DataTable ss = db.List(sql);
            var list = new Staff().List(ss);

            ViewBag.StaffID = new SelectList(list, "StaffID", "StaffDetail");

            List<Dictionary<string,string>> emp = new List<Dictionary<string,string>>();
            ViewBag.Details = emp;
            ViewBag.TotalHr = "";

            return View();
        }

        [HttpPost]
        public ActionResult TotalWorkWeekHR(string StaffID)
        {
            Debug.WriteLine(StaffID);
            string sql = "Select s.FirstName, s.LastName, c.CourseID, c.Title AS CourseTitle, j.JobID, j.Title AS JobTitle,s.ContactNo,s.Email, sch.StartTime,sch.EndTime from Schedules sch JOIN Courses c ON c.CourseID = sch.CourseID JOIN StaffSchedules ss ON ss.ScheduleID = sch.ScheduleID JOIN Staffs s ON s.StaffID = ss.StaffID JOIN JobStaffs js on js.StaffID = s.StaffID JOIN Jobs j on j.JobID = js.JobID WHERE s.StaffID='" + StaffID + "' Order BY c.Title";
            DataTable dt = db.List(sql);
            Debug.WriteLine(dt.Rows.Count);


            TimeSpan totalHours = new TimeSpan();


            List<Dictionary<string, string>> aa = new List<Dictionary<string, string>>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("FirstName", dt.Rows[i]["FirstName"].ToString());
                dic.Add("LastName", dt.Rows[i]["LastName"].ToString());
                dic.Add("CourseDetail", dt.Rows[i]["CourseID"].ToString() + "("+ dt.Rows[i]["CourseTitle"].ToString() + ")" );
                dic.Add("JobDetail", dt.Rows[i]["JobID"].ToString() + "(" + dt.Rows[i]["JobTitle"].ToString() + ")");
                dic.Add("ContactNo", dt.Rows[i]["ContactNo"].ToString());
                dic.Add("Email", dt.Rows[i]["Email"].ToString());
                dic.Add("StartTime", Convert.ToDateTime(dt.Rows[i]["StartTime"].ToString()).TimeOfDay.ToString());
                dic.Add("EndTime", Convert.ToDateTime(dt.Rows[i]["EndTime"].ToString()).TimeOfDay.ToString());

                aa.Add(dic);
                DateTime startTime = Convert.ToDateTime(dt.Rows[i]["StartTime"].ToString());
               DateTime endTime = Convert.ToDateTime(dt.Rows[i]["EndTime"].ToString());
              
                TimeSpan diff = (endTime.TimeOfDay - startTime.TimeOfDay);

                totalHours += diff;
              


            }
            ViewBag.TotalHr = totalHours.ToString();

            string sqlSelectList = "Select s.StaffID, s.FirstName, s.LastName,s.ContactNo,s.DOB, j.Title as JobTitle from Staffs s JOIN JobStaffs js ON js.StaffID = s.StaffID JOIN Jobs j ON j.JobID = js.JobID Where j.Title IN ('Lecturer','Tutor')";
            DataTable ss = db.List(sqlSelectList);
            var list = new Staff().List(ss);
            ViewBag.StaffID = new SelectList(list, "StaffID", "StaffDetail");
            ViewBag.Details = aa;
            return View();
        }

        public ActionResult AbsentWeek()
        {
            string sql = "Select * from Groups g JOIN Faculties f ON f.FacultyID =g.FacultyID";
            DataTable dt = db.List(sql);
            var list = new Group().List(dt);
            ViewBag.GroupID = new SelectList(list, "GroupID", "GroupDetail");


            var studentlist = new List<Attendance>();
            return View(studentlist);


        }

        [HttpPost]
        public ActionResult AbsentWeek(string GroupID, string Date)
        {
            DateTime ss = Convert.ToDateTime(Date);
            DateTime startDay = FirstDayOfWeek(ss);
            DateTime endDay = startDay.AddDays(7);

            string sql = "Select s.FirstName, s.LastName, c.Title, a.CourseID,a.Status,a.AttendanceDate from Attendances a JOIN Students s ON a.StudentID = s.StudentID JOIN Courses c ON c.CourseID = a.CourseID Where s.GroupID='" + GroupID + "' and a.AttendanceDate Between '" + startDay + "' and '" + endDay + "' and a.Status='Absent'";
            DataTable dt = db.List(sql);

            string sqllist = "Select * from Groups g JOIN Faculties f ON f.FacultyID =g.FacultyID";
            DataTable dtlist = db.List(sqllist);
            var GList = new Group().List(dtlist);
            ViewBag.GroupID = new SelectList(GList, "GroupID", "GroupDetail");
            if (dt.Rows.Count > 0)
            {
                var list = new Attendance().List(dt);
                return View(list);
            }
            else
            {
                var list = new List<Attendance>();
                return View(list);

            }
    
        }
    }
}