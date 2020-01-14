using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models
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
        [StringLength(4000)]
        public string Body { get; set; }
    }
}
