using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Web;

namespace UserAuthentication.Models
{
    public class Faculty
    {
        List<Faculty> list = new List<Faculty>();


        public List<Faculty> List(DataTable dt)
        {

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Faculty fac = new Faculty();
                fac.FacultyID = dt.Rows[i]["FacultyID"].ToString();
                fac.Name = dt.Rows[i]["Name"].ToString();
                fac.Description = dt.Rows[i]["Description"].ToString();
                list.Add(fac);
            }
            return list;

        }

        [Key]
        public string FacultyID { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

    }
}