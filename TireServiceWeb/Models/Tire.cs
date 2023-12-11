using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TireServiceWeb.Models
{
    public class Tire
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Size { get; set; }
        public int Quantity { get; set; }
        [ForeignKey("TireShop")]
        public int TireShopId { get; set; }
        public TireShop? TireShop { get; set; }
    }
}
