using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Web;

namespace UserAuthentication.Models
{
    public class Assignment
    {
        [Key]
        public int AssignmentID { get; set; }

        public string CourseID { get; set; }

        [ForeignKey("CourseID")]
        public virtual Course course { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string AssignmentType { get; set; }

        List<Assignment> list = new List<Assignment>();

        public List<Assignment> List(DataTable dataTable)
        {
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                Assignment assignment = new Assignment();
                assignment.AssignmentID = Convert.ToInt32(dataTable.Rows[i]["AssignmentID"].ToString());
                assignment.CourseID = dataTable.Rows[i]["CourseID"].ToString();
                assignment.Title = dataTable.Rows[i]["Title"].ToString();
                assignment.AssignmentType = dataTable.Rows[i]["AssignmentType"].ToString();
                list.Add(assignment);
            }
            return list;
        }
    }
}