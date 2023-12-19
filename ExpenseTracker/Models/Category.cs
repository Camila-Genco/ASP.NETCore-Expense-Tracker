using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string Name { get; set; }

        [Column(TypeName = "nvarchar(5)")]
        public string Icon { get; set; } = "";

        [Column(TypeName = "nvarchar(10)")]
        public string Type { get; set; } = "Expense";

        [NotMapped]
        public string? NameAndIcon
        {
            get
            {
                return this.Icon + " " + this.Name;
            }
        }
    }
}
