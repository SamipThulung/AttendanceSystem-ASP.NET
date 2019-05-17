using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Web;

namespace UserAuthentication.Models
{
    public class ScheduleGroup
    {
        [Key, Column(Order = 1)]
        public int ScheduleID { get; set; }

        [ForeignKey("ScheduleID")]
        public virtual Schedule schedule { get; set; }

        [Key, Column(Order = 2)]
        public string GroupID { get; set; }

        [ForeignKey("GroupID")]
        public virtual Group group { get; set; }

        List<ScheduleGroup> list = new List<ScheduleGroup>();

        public List<ScheduleGroup> List(DataTable dataTable)
        {
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                ScheduleGroup scheduleGroup = new ScheduleGroup();
                scheduleGroup.ScheduleID = Convert.ToInt32(dataTable.Rows[i]["ScheduleID"].ToString());
                scheduleGroup.GroupID = dataTable.Rows[i]["GroupID"].ToString();
                list.Add(scheduleGroup);
            }
            return list;
        }

    }
}