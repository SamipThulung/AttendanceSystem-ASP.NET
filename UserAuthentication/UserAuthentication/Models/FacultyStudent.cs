using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Web;

namespace UserAuthentication.Models
{
    public class FacultyStudent
    {
        List<FacultyStudent> list = new List<FacultyStudent>();


        public List<FacultyStudent> List(DataTable dt)
        {

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                FacultyStudent facultyStudent = new FacultyStudent();
                facultyStudent.FacultyID = dt.Rows[i]["FacultyID"].ToString();
                facultyStudent.StudentID = dt.Rows[i]["StudentID"].ToString();
                list.Add(facultyStudent);
            }
            return list;
        }

        [Key, Column(Order = 1)]
        public string FacultyID { get; set; }

        [ForeignKey("FacultyID")]
        public virtual Faculty faculty { get; set; }

        [Key, Column(Order = 2)]
        public string StudentID { get; set; }

        [ForeignKey("StudentID")]
        public virtual Student student { get; set; }
    }
}