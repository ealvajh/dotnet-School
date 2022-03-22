using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Schooltt.Models
{
    public class StudentModel
    {
        public int StudentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set;}
        public int? StandardId { get; set; } 

        public AddressModel Address { get; set; }   
        public StandardModel Standard { get; set; }
    }
}