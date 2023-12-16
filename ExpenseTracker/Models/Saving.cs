using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseTracker.Models
{
    public class Saving
    {
        [Key]
        public int SavingId { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string SavingName { get; set; }
        public int Amount { get; set; }
        public int? GoalAmount { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [NotMapped]
        public string? DateFormat => this.CreatedAt.ToString("dd/MM/yyyy");

        [NotMapped]
        public int? Percentage => Convert.ToInt32(((double)this.Amount / this.GoalAmount) * 100);
    }
}
