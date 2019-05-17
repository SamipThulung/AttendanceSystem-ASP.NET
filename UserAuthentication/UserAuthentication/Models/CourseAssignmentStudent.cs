using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Web;

namespace UserAuthentication.Models
{
    public class CourseAssignmentStudent
    {
        [Key, Column(Order = 1)]
        public string CourseID { get; set; }

        [ForeignKey("CourseID")]
        public virtual Course course { get; set; }

        [Key, Column(Order = 2)]
        public int AssignmentID { get; set; }

        [ForeignKey("AssignmentID")]
        public virtual Assignment assignment { get; set; }

        [Required]
        public string TotalMarks { get; set; }
    
        List<CourseAssignmentStudent> list = new List<CourseAssignmentStudent>();

        public List<CourseAssignmentStudent> List(DataTable dataTable)
        {
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                CourseAssignmentStudent courseAssignmentStudent = new CourseAssignmentStudent();
                courseAssignmentStudent.CourseID = dataTable.Rows[i]["CourseID"].ToString();
                courseAssignmentStudent.AssignmentID = Convert.ToInt32(dataTable.Rows[i]["AssignmentID"].ToString());
                courseAssignmentStudent.TotalMarks = dataTable.Rows[i]["TotalMarks"].ToString();
                list.Add(courseAssignmentStudent);
            }
            return list;
        }
    }
}