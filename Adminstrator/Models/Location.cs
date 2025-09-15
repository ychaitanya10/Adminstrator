using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Adminstrator.Models
{
    public class Location
    {
        [Key]
        public int LocationId { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string State { get; set; }
        public string Description { get; set; }
        [JsonIgnore]
        public ICollection<Event> ?Events { get; set; }
    }
}
