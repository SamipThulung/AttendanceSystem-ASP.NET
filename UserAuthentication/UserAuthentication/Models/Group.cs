using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace UserAuthentication.Models
{
    public class Group
    {
        List<Group> list = new List<Group>();


        public List<Group> List(DataTable dt)
        {

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Group group = new Group();
                group.GroupID = dt.Rows[i]["GroupID"].ToString();
                group.Semester = dt.Rows[i]["Semester"].ToString();
                group.Year = dt.Rows[i]["Year"].ToString();
                group.FacultyID = dt.Rows[i]["FacultyID"].ToString();
                group.Fac = new Faculty();
                group.FacName = dt.Rows[i]["Name"].ToString();
                //Debug.WriteLine("Facultyid ===========================>");
                //Debug.WriteLine(dt.Rows[i]["FacultyID"]);
                list.Add(group);
            }
            return list;
        }

        [Key]
        public string GroupID { get; set; }

        [NotMapped]
        public string FacName { get; set; }

        [Required]
        public string Semester { get; set; }

        [Required]
        public string Year { get; set; }

        public string FacultyID { get; set; }

        [ForeignKey("FacultyID")]
        public virtual Faculty Fac { get; set; }

        [NotMapped]
        public string GroupDetail { get { return GroupID + ": " + FacName + " Semester: " + Semester + " Year: " + Year; } }
    }
}