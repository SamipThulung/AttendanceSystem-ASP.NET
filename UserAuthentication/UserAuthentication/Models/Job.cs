using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Web;

namespace UserAuthentication.Models
{
    public class Job
    {
        [Key]
        public int JobID { get; set; }

        [Required]
        public string Title { get; set; }

        List<Job> list = new List<Job>();

        public List<Job> List(DataTable dataTable)
        {
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                Job job = new Job();
                job.JobID = Convert.ToInt32(dataTable.Rows[i]["JobID"].ToString());
                job.Title = dataTable.Rows[i]["Title"].ToString();
                list.Add(job);
            }
            return list;
        }
    }
}