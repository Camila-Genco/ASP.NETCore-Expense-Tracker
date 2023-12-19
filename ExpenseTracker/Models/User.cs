using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseTracker.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string UserName { get; set; } = "Jane Doe";
        public string UserEmail { get; set; } = "example@mail.com";
        public string Password { get; set; } = "contrasena123";
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
