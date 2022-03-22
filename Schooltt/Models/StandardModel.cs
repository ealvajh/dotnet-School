using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Schooltt.Models
{
    public class StandardModel
    {
        public int StandardId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public ICollection<StudentModel> Students { get; set;}
    }
}