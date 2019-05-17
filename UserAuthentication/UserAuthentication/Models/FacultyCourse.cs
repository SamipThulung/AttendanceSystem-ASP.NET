using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Web;

namespace UserAuthentication.Models
{
    public class FacultyCourse
    {

        List<FacultyCourse> list = new List<FacultyCourse>();


        public List<FacultyCourse> List(DataTable dt)
        {

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                FacultyCourse facultyCourse = new FacultyCourse();
                facultyCourse.FacultyID = dt.Rows[i]["FacultyID"].ToString();
                facultyCourse.CourseID = dt.Rows[i]["CourseID"].ToString();
                list.Add(facultyCourse);
            }
            return list;
        }
        [Key, Column(Order = 1)]
        public string FacultyID { get; set; }

        [ForeignKey("FacultyID")]
        public virtual Faculty Fac { get; set; }

        [Key, Column(Order = 2)]
        public string CourseID { get; set; }

        [ForeignKey("CourseID")]
        public virtual Course course { get; set; }
    }
}