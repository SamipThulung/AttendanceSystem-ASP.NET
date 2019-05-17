using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Web;

namespace UserAuthentication.Models
{
    public class Attendance
    {
        [Key]
        public int AttendanceID { get; set; }

        public int ScheduleID { get; set; }

        [ForeignKey("ScheduleID")]
        public virtual Schedule schedule { get; set; }


        public string CourseID { get; set; }

        [ForeignKey("CourseID")]
        public virtual Course course { get; set; }

        [Required]
        public string Status { get; set; }


        public string Record { get; set; }


        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime AttendanceDate { get; set; }

        public string StudentID { get; set; }

        [ForeignKey("StudentID")]
        public virtual Student std { get; set; }

        [NotMapped]
        public string FirstName { get; set; }

        [NotMapped]
        public string LastName { get; set; }
        [NotMapped]
        public string Title { get; set; }

        [NotMapped]
        public string Count { get; set; }

        List<Attendance> list = new List<Attendance>();


        public List<Attendance> List(DataTable dt)
        {

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Attendance student = new Attendance();

                student.FirstName = dt.Rows[i]["FirstName"].ToString();
                student.LastName = dt.Rows[i]["LastName"].ToString();
                student.Title = dt.Rows[i]["Title"].ToString();
                student.CourseID = dt.Rows[i]["CourseID"].ToString();
                student.Status = dt.Rows[i]["Status"].ToString();
                student.AttendanceDate = Convert.ToDateTime(dt.Rows[i]["AttendanceDate"].ToString());
                list.Add(student);
            }
            return list;

        }
        public List<Attendance> ListToCount(DataTable dt)
        {

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Attendance student = new Attendance();

             
                student.Title = dt.Rows[i]["Title"].ToString();
                student.Count = dt.Rows[i]["Present"].ToString();
                student.Status = dt.Rows[i]["Status"].ToString();
                list.Add(student);
            }
            return list;

        }
    }
}