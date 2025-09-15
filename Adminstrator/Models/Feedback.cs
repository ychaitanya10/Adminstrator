using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Adminstrator.Models
{
    public class Feedback
    {
        [Key]
        public int FeedBackID { get; set; }

        [Required]
        public int EventID { get; set; }
        [ForeignKey("EventID")]
        public Event Event { get; set; } 

        [Required]
        public int SpeakerID { get; set; }
        [ForeignKey("SpeakerID")]
        public Speaker Speaker { get; set; } 

        [Required]
        public string feedback_remarks { get; set; }
    }
}
