using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiTgBot.Models
{
    [Table("Cities")]
    public class City
    {
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
