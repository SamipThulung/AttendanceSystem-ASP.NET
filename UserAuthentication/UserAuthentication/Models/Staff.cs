using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Web;

namespace UserAuthentication.Models
{
    public class Staff
    {
        List<Staff> list = new List<Staff>();

        public List<Staff> List(DataTable dt)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Staff staff = new Staff();
                DataColumn[] keyColumns = new DataColumn[1];
                keyColumns[0] = dt.Columns["StaffID"];

                dt.PrimaryKey = keyColumns;
                staff.StaffID = dt.Rows[i]["StaffID"].ToString();
                staff.FirstName = dt.Rows[i]["FirstName"].ToString();
                staff.LastName = dt.Rows[i]["LastName"].ToString();
                staff.ContactNo = dt.Rows[i]["ContactNo"].ToString();
                staff.DOB = Convert.ToDateTime(dt.Rows[i]["DOB"]);
                if (dt.Columns.Contains("JobTitle"))
                { 
                  staff.JobTitle = dt.Rows[i]["JobTitle"].ToString();
                }
                list.Add(staff);
            }
            return list;
        }

    [Key]
        public string StaffID { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string ContactNo { get; set; }

        public string Email { get; set; }

        [DataType(DataType.Date)]
        public DateTime DOB { get; set; }

        public int JobID { get; set; }

        [ForeignKey("JobID")]
        public virtual Job Job { get; set; }

        [NotMapped]
        public string StaffDetail { get { return StaffID + ": " + FirstName + " " + LastName + " (" + JobTitle + ")"; } }

        [NotMapped]
        public string JobTitle { get; set; }
    }
}