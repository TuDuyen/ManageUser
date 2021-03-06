using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ManageUser_API.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Fullname { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public bool isActive { get; set; }
    }
}
