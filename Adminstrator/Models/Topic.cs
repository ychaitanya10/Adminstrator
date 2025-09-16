using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Adminstrator.Models
{
    public class Topic
    {
        [Key]
        public int TopicId { get; set; }
        [Required]
        public string TopicCode { get; set; }
        [Required]
        public string TopicName { get; set; }
        [Required]
        public string Category { get; set; }
        public string Description { get; set; }
        [JsonIgnore]
        public ICollection<Event> Events { get; set; } = new List<Event>();
    }
}
