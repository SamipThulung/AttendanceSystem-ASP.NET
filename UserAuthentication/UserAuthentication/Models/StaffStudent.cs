using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Web;

namespace UserAuthentication.Models
{
    public class StaffStudent
    {
        List<StaffStudent> list = new List<StaffStudent>();

        public List<StaffStudent> List(DataTable dt)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                StaffStudent staffStudent = new StaffStudent();
                staffStudent.StaffID = dt.Rows[i]["StaffID"].ToString();
                staffStudent.StudentID = dt.Rows[i]["StudentID"].ToString();
                list.Add(staffStudent);
            }
            return list;
        }

        [Key, Column(Order = 1)]
        public string StaffID { get; set; }

        [ForeignKey("StaffID")]
        public virtual Staff staff { get; set; }

        [Key, Column(Order = 2)]
        public string StudentID { get; set; }

        [ForeignKey("StudentID")]
        public virtual Student student { get; set; }
    }
}