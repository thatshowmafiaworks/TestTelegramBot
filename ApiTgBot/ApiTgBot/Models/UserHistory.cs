using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiTgBot.Models
{
    [Table("UserHistories")]
    public class UserHistory
    {
        public long Id { get; set; }
        [Required]
        public long UserId { get; set; }
        public IEnumerable<HistoryRecord>? Records { get; set; }
        public User? User { get; set; }
    }
}
