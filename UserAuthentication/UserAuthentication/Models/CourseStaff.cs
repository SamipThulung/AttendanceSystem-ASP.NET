using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Web;

namespace UserAuthentication.Models
{
    public class CourseStaff
    {
        List<CourseStaff> list = new List<CourseStaff>();

        public List<CourseStaff> List(DataTable dt)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                CourseStaff courseStaff = new CourseStaff();
                courseStaff.CourseID = dt.Rows[i]["CourseID"].ToString();
                courseStaff.StaffID = dt.Rows[i]["StaffID"].ToString();

                courseStaff.CourseName = dt.Rows[i]["Title"].ToString();
                courseStaff.TeacherDetail = dt.Rows[i]["StaffID"].ToString() + " " + dt.Rows[i]["FirstName"].ToString() + " "+ dt.Rows[i]["LastName"].ToString() + " ( "+ dt.Rows[i]["JobTitle"].ToString() + ")";
                list.Add(courseStaff);
            }
            return list;
        }

        [Key, Column(Order = 1)]
        public string StaffID { get; set; }

        [ForeignKey("StaffID")]
        public virtual Staff staff { get; set; }

        [Key, Column(Order = 2)]
        public string CourseID { get; set; }

        [ForeignKey("CourseID")]
        public virtual Course course { get; set; }

        public string Semester { get; set; }

        [NotMapped]
        public string CourseName { get; set; }

        [NotMapped]
        public string TeacherDetail { get; set; }
    }
}