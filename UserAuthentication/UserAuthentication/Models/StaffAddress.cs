using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace UserAuthentication.Models
{
    public class StaffAddress
    {
        [Key, Column(Order = 1)]
        public string StaffID { get; set; }

        [Key, Column(Order = 2)]
        public int AddressID { get; set; }

        [ForeignKey("StaffID")]
        public virtual Staff staff { get; set; }

        [ForeignKey("AddressID")]
        public virtual Address address { get; set; }
    }
}