using System;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models
{
    /// <summary>
    /// User
    /// </summary>
    public class User
    {
        /// <summary>
        /// User id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// User First Name
        /// </summary>
        [Required]
        [StringLength(45, MinimumLength = 1)]
        public string FirstName { get; set; }
        /// <summary>
        /// User Last Name
        /// </summary>
        [Required]
        [StringLength(45, MinimumLength = 1)]
        public string LastName { get; set; }
        /// <summary>
        /// User Email
        /// </summary>
        [Required]
        [StringLength(45)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        /// <summary>
        /// User Password
        /// </summary>
        [Required]
        [StringLength(45, MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        /// <summary>
        /// User Date Of Birth
        /// </summary>
        [Required]
        [DataType(DataType.Date)]
        public DateTime DoB { get; set; }
    }
}
