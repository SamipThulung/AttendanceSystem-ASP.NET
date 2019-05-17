using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace UserAuthentication.Models
{
   

    public class Institute
    {
        List<Institute> list = new List<Institute>();


        public List<Institute> List(DataTable dt)
        {

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Institute institute = new Institute();
                institute.InstituteID = Convert.ToInt32(dt.Rows[i]["InstituteID"]);
                institute.Name = dt.Rows[i]["Name"].ToString();
                list.Add(institute);
            }
            return list;
        }
        public int InstituteID { get; set; }

        public string Name { get; set; }
    }
}