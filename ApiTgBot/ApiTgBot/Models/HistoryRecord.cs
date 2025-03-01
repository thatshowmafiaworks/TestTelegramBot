using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiTgBot.Models
{
    [Table("HistoryRecords")]
    public class HistoryRecord
    {
        [Key]
        public long Id { get; set; }
        [Required]
        public string Text { get; set; }
        [Required]
        public long HistoryId { get; set; }
        public DateTime DateTime { get; set; }

        public UserHistory History { get; set; }
    }
}
