using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Web;

namespace UserAuthentication.Models
{
    public class Departments
    {
        [Key]
        public int DepartmentID { get; set; }

        [Required]
        public string DepartmentName { get; set; }

        List<Departments> list = new List<Departments>();

        public List<Departments> List(DataTable dataTable)
        {
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                Departments departments = new Departments();
                departments.DepartmentID = Convert.ToInt32(dataTable.Rows[i]["DepartmentID"].ToString());
                departments.DepartmentName = dataTable.Rows[i]["DepartmentName"].ToString();
                list.Add(departments);
            }
            return list;
        }
    }
}