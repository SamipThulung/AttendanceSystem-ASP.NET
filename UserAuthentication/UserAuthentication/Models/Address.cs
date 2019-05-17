using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Web;

namespace UserAuthentication.Models
{
    public class Address
    {
        List<Address> list = new List<Address>();


        public List<Address> List(DataTable dt)
        {

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Address address = new Address();
                address.AddressID = Convert.ToInt32(dt.Rows[i]["AddressID"]);
                address.AddressName = dt.Rows[i]["AddressName"].ToString();
                list.Add(address);
            }
            return list;
        }

        [Key]
        public int AddressID { get; set; }

        [Required]
        public string AddressName { get; set; }
    }
}