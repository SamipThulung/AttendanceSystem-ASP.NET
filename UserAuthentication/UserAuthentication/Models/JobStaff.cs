using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Web;

namespace UserAuthentication.Models
{
    public class JobStaff
    {

        [Key, Column(Order =1)]
        public string StaffID { get; set; }
        [Key, Column(Order = 2)]
        public int JobID { get; set; }

        [ForeignKey("JobID")]
        public virtual Job job { get; set; }

        [ForeignKey("StaffID")]
        public virtual Staff staff { get; set; }

        List<JobStaff> list = new List<JobStaff>();

        public List<JobStaff> List(DataTable dataTable)
        {
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                JobStaff jobStaff = new JobStaff();
                jobStaff.StaffID = dataTable.Rows[i]["StaffID"].ToString();
                jobStaff.JobID = Convert.ToInt32(dataTable.Rows[i]["JobID"].ToString());
                list.Add(jobStaff);
            }
            return list;
        }

    }
}