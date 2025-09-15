using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Adminstrator.Models
{
    public class Administrator
    {
        [Key]
        public int AdminId { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 8)]
        public string Name { get; set; }    
        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        public string Email { get; set; }
        [Required]
        [RegularExpression(@"^[6-9]\d{9}$")]
        public string Phone { get; set; }


        // Link to User for login
        [ForeignKey("UserId")]
        public int UserId { get; set; }
        [JsonIgnore]
        public User? User { get; set; }
    }
}
