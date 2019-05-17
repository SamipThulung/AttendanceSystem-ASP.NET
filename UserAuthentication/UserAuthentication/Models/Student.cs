using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Web;

namespace UserAuthentication.Models
{
    public class Student
    {
        [Key]
        public string StudentID { get; set; }

 
        public string FirstName { get; set; }

 
        public string LastName { get; set; }

    
        public string Gender { get; set; }


        public string Email { get; set; }


        public string ContactNumber { get; set; }

   
        public string CurrentAddress { get; set; }

 
        public string PermanentAddress { get; set; }

        public string GroupID { get; set; }

        [ForeignKey("GroupID")]
        public virtual Group group { get; set; }

        

        [NotMapped]
        public string Semester;

        [DataType(DataType.Date)]
        public DateTime EnrollDate { get; set; }

        [NotMapped]
        public string CurrentStatus { get; set; }

        [NotMapped]
        public string StudentDetail
        {
            get { return StudentID + ": " + FirstName + " " + LastName + " (" + GroupID + ")"; }
        }

        List<Student> list = new List<Student>();


        public List<Student> List(DataTable dt)
        {

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Student student = new Student();
                student.StudentID = dt.Rows[i]["StudentID"].ToString();
                student.FirstName = dt.Rows[i]["FirstName"].ToString();
                student.LastName = dt.Rows[i]["LastName"].ToString();
                student.Gender = dt.Rows[i]["Gender"].ToString();
                student.Email = dt.Rows[i]["Email"].ToString();
                student.ContactNumber = dt.Rows[i]["ContactNumber"].ToString();
                student.CurrentAddress = dt.Rows[i]["CurrentAddress"].ToString();
                student.PermanentAddress = dt.Rows[i]["PermanentAddress"].ToString();
                student.GroupID = dt.Rows[i]["GroupID"].ToString();
 
                student.Semester = dt.Rows[i]["Semester"].ToString();
                student.EnrollDate = Convert.ToDateTime(dt.Rows[i]["EnrollDate"]);
                list.Add(student);
            }
            return list;
        }
    }
}