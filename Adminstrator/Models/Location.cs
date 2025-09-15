using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public ICollection<Event> Events { get; set; }
    }
}
