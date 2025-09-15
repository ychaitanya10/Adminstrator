using System.ComponentModel.DataAnnotations;

namespace Adminstrator.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 6)]
        public string Username { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 6)]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]$",
        ErrorMessage = "Password must be 6-20 characters, contain at least one letter and one number.")]
        public string Password { get; set; }
        [Required]
        public string Role { get; set; } 

        public Participant ParticipantProfile { get; set; }
        public Speaker SpeakerProfile { get; set; }
        public Administrator AdministratorProfile { get; set; }
    }
}
