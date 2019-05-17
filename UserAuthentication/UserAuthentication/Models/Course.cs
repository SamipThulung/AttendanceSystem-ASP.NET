using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Web;

namespace UserAuthentication.Models
{
    public class Course
    {
        [Key]
        public string CourseID { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Period { get; set; }

        [Required]
        public string Level { get; set; }

        [Required]
        public int CreditHour { get; set; }

        List<Course> list = new List<Course>();

        public List<Course> List(DataTable dataTable)
        {
            for (int i = 0; i < dataTable.Rows.Count; i ++)
            {
                Course course = new Course();
                course.CourseID = dataTable.Rows[i]["CourseID"].ToString();
                course.Title = dataTable.Rows[i]["Title"].ToString();
                course.Period = dataTable.Rows[i]["Period"].ToString();
                course.Level = dataTable.Rows[i]["Level"].ToString();
                course.CreditHour = Convert.ToInt32(dataTable.Rows[i]["CreditHour"].ToString());
                list.Add(course);
            }
            return list;
        }

        public string FacultyID { get; set; }

        [ForeignKey("FacultyID")]
        public virtual Faculty Fac { get; set; }
    }
}