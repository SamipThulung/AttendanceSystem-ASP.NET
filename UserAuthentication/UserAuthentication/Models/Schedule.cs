using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Web;

namespace UserAuthentication.Models
{
    public class Schedule
    {
        [Key]
        public int ScheduleID { get; set; }

        public string CourseID { get; set; }

        [ForeignKey("CourseID")]
        public virtual Course course { get; set; }

        [Required]
        public string Classroom { get; set; }

        [Required]
        public string ClassType { get; set; }

        [Required]
        public string Day { get; set; }

        [Required]
        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:H:mm}")]
        public DateTime StartTime { get; set; }

        [Required]
        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:H:mm}")]
        public DateTime EndTime { get; set; }

        [NotMapped]
        public string FullSchedule
        {
            get
            {
                return String.Format("{0}. {1} ({2})", ScheduleID, Title, ClassType);
            }
        }
        [NotMapped]
        public string Title { get; set; }

        [NotMapped]
        public string CourseTitle { get; set; }
        List<Schedule> list = new List<Schedule>();

        public List<Schedule> List(DataTable dataTable)
        {
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                Schedule schedule = new Schedule();
                schedule.ScheduleID = Convert.ToInt32(dataTable.Rows[i]["ScheduleID"].ToString());
                schedule.CourseID = dataTable.Rows[i]["CourseID"].ToString();
                schedule.Classroom = dataTable.Rows[i]["Classroom"].ToString();
                schedule.ClassType = dataTable.Rows[i]["ClassType"].ToString();
                schedule.Day = dataTable.Rows[i]["Day"].ToString();
                schedule.StartTime = Convert.ToDateTime(dataTable.Rows[i]["StartTime"].ToString());
                schedule.EndTime = Convert.ToDateTime(dataTable.Rows[i]["EndTime"].ToString());
                if (dataTable.Columns.Contains("CourseTitle"))
                {
                    
                    schedule.CourseTitle = dataTable.Rows[i]["CourseTitle"].ToString();
                }
                list.Add(schedule);
            }
            return list;
        }

    }

}