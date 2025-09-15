using System.ComponentModel.DataAnnotations;

namespace electronica.Models
{
    public class NotificationMod
    {
        [Key]
        public int NotifID { get; set; }
        public string RecipientEmail { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public bool IsSent { get; set; }
        public string Role { get; set; }

    }
}
