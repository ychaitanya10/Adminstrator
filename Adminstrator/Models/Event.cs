using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Adminstrator.Models
{
    public class Event
    {
        [Key]
        public int EventId { get; set; }
        [Required]
        public string CourseTitle { get; set; }

        [ForeignKey("TopicId")]
        public int TopicId { get; set; }
        [JsonIgnore]
        public Topic ?Topic { get; set; }

        [ForeignKey("LocationId")]
        public int LocationId { get; set; }
        [JsonIgnore]
        public Location? Location { get; set; }

        [ForeignKey("SpeakerId")]
        public int SpeakerId { get; set; }
        [JsonIgnore]
        public Speaker? Speaker { get; set; }

        [Required]
        public int ClassSize { get; set; }
        [Required]
        public int NumberOfDays { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }

        // Many-to-many: Participants attending this event
        [JsonIgnore]
        public ICollection<Participant> Participants { get; set; } = new List<Participant>();

        // One-to-many: Feedbacks for this event
        [JsonIgnore]
        public ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();
    }
}
