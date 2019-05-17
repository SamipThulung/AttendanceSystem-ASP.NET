using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Web;

namespace UserAuthentication.Models
{
    public class CourseStudent
    {
        [Key, Column(Order = 1)]
        public string StudentID { get; set; }

        [ForeignKey("StudentID")]
        public virtual Student student { get; set; }

        [Key, Column(Order = 2)]
        public string CourseID { get; set; }

        [ForeignKey("CourseID")]
        public virtual Course course { get; set; }

      
        public string TotalMarks { get; set; }

        public string CompletedYear { get; set; }

        [NotMapped]
        public string CourseTitle { get; set; }


        [NotMapped]
        public string StudentName { get; set; }

        List<CourseStudent> list = new List<CourseStudent>();

        public List<CourseStudent> List(DataTable dataTable)
        {
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                CourseStudent courseStudent = new CourseStudent();
                courseStudent.StudentID = dataTable.Rows[i]["StudentID"].ToString();
                courseStudent.CourseID = dataTable.Rows[i]["CourseID"].ToString();
                courseStudent.TotalMarks = dataTable.Rows[i]["TotalMarks"].ToString();
                courseStudent.CompletedYear = dataTable.Rows[i]["CompletedYear"].ToString();
                if (dataTable.Columns.Contains("CourseTitle"))
                {
                    courseStudent.CourseTitle = dataTable.Rows[i]["CourseTitle"].ToString();
                }
                if(dataTable.Columns.Contains("StudentName")){
                    courseStudent.StudentName = dataTable.Rows[i]["StudentName"].ToString();
                }
                list.Add(courseStudent);
            }
            return list;
        }
    }
}