using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Adminstrator.Models
{
    public class Event
    {
        [Key]
        public int EventId { get; set; }
        [Required]
        public string CourseTitle { get; set; }

        
        public int TopicId { get; set; }
        [ForeignKey("TopicId")]
        public Topic Topic { get; set; }

        public int LocationId { get; set; }
        [ForeignKey("LocationId")]
        public Location Location { get; set; }

        public int SpeakerId { get; set; }
        [ForeignKey("SpeakerId")]
        public Speaker Speaker { get; set; }

        [Required]
        public int ClassSize { get; set; }
        [Required]
        public int NumberOfDays { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }

        // Many-to-many: Participants attending this event
        public ICollection<Participant> Participants { get; set; }

        // One-to-many: Feedbacks for this event
        public ICollection<Feedback> Feedbacks { get; set; }
    }
}
