using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiTgBot.Models
{
    [Table("Users")]
    public class User
    {
        public long Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        [Required]
        public string? Username { get; set; }
        public long CityId { get; set; }
        public long ChatId { get; set; }

        public float Lat { get; set; }
        public float Lng { get; set; }
        public City City { get; set; }
        public IEnumerable<UserHistory> Histories{get;set;}
    }
}
