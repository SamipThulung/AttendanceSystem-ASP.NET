using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Web;

namespace UserAuthentication.Models
{
    public class DepartmentStaff
    {
        [Key, Column(Order = 1)]
        public int DepartmentID { get; set; }

        [Key, Column(Order = 2)]
        public string StaffID { get; set; }

        [ForeignKey("DepartmentID")]
        public virtual Departments departments { get; set; }

        [ForeignKey("StaffID")]
        public virtual Staff staff { get; set; }
        List<DepartmentStaff> list = new List<DepartmentStaff>();

        public List<DepartmentStaff> List(DataTable dataTable)
        {
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                DepartmentStaff departmentStaff = new DepartmentStaff();
                departmentStaff.DepartmentID = Convert.ToInt32(dataTable.Rows[i]["DepartmentID"].ToString());
                departmentStaff.StaffID = dataTable.Rows[i]["StaffID"].ToString();
                list.Add(departmentStaff);
            }
            return list;
        }
    }
}