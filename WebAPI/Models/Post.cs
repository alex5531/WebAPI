using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models
{
    /// <summary>
    /// Post
    /// </summary>
    public class Post
    {
        /// <summary>
        /// Post id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// User Id
        /// </summary>
        [Required]
        public int UserId { get; set; }
        /// <summary>
        /// Title 
        /// </summary>
        [StringLength(255)]
        public string Title { get; set; }
        /// <summary>
        /// Body
        /// </summary>
        [StringLength(4000)]
        public string Body { get; set; }
    }
}
