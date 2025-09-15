using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace electronica.Models
{
    public class FeedbackMod
    {
        [Key]
        public int FeedBackID { get; set; }
        public int EventID { get; set; }
        public int ParticipantID { get; set; }
        public string Question { get; set; } = "Please submit your valuable feedback about the course and the speaker.";
        [Required]
        public string Answer { get; set; }
        [Required]
        public int Rating { get; set; }


        public Event Event { get; set; }
        public Participant Participant { get; set; }


    }
}
