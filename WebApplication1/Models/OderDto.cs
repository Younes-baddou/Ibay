using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class OderDto
    {
        [Required]
        public string ProductIndentifiers { get; set; } = "";

        [Required,MinLength(30),MaxLength(100)]
        public string DeliveryAddress { get; set; } = "";

        [Required]
        public string PaymentMethod { get; set; } = "";
    }
}
