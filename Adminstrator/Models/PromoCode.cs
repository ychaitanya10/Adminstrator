using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Adminstrator.Models
{
    public class PromotionCode
    {
        [Key]
        public int PromotionCodeId { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public DateOnly ValidFrom { get; set; }
        [Required]
        public DateOnly ValidTo { get; set; }
        [Required]
        public decimal DiscountPercentage { get; set; }
    }
}
