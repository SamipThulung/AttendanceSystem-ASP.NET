using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace UserAuthentication.Models
{
    public class StaffSchedule
    {
        [Key, Column(Order = 1)]
        public string StaffID { get; set; }

        [ForeignKey("StaffID")]
        public virtual Staff staff { get; set; }

        [Key, Column(Order = 2)]
        public string CourseID { get; set; }

        [ForeignKey("CourseID")]
        public virtual Course course { get; set; }

        public int ScheduleID { get; set; }

        [ForeignKey("ScheduleID")]
        public virtual Schedule schedule { get; set; }
    }
}