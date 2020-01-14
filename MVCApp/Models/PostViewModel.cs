using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MVCApp.Models
{
    public class PostViewModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [StringLength(255)]
        public string Title { get; set; }
        [Display(Name = "Message")]
        [StringLength(4000)]
        public string Body { get; set; }
    }
}
