using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Models
{
    public class Transaction
    {
        [Key]
        public int TransactionId { get; set; }
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
        public int Amount { get; set; }

        [Column(TypeName = "nvarchar(75)")]
        public string? Description { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;

        [NotMapped]
        public string? DateFormat => this.Date.ToString("MM/dd/yyyy");

        [NotMapped]
        public string? CategoryAndIcon
        {
            get
            {
                return Category?.Icon + "" + Category?.Name;
            }
        }

        [NotMapped]
        public string? AmountFormat
        {
            get
            {
                return ((Category == null || Category.Type == "Expense") ? "- " : "+ ") + Amount.ToString("C0");
            }
        }
    }
}
