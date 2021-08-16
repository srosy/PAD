using System;
using System.ComponentModel.DataAnnotations;
namespace PAD.Data.Models
{
    public class AspNetUsers : BaseModel
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string NormalizedUserName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string NormalizedEmail { get; set; }
        [Required]
        public bool EmailConfirmed { get; set; }
        [Required]
        private string PasswordHash { get; set; }
        [Required]
        private string SecurityStamp { get; set; }
        [Required]
        private string ConcurrencyStamp { get; set; }
        public string PhoneNumber { get; set; }
        [Required]
        public bool PhoneNumberConfirmed { get; set; }
        [Required]
        public bool TwoFactorEnabled { get; set; }
        public DateTime LockoutEnd { get; set; }
        [Required]
        public bool LockOutEnabled { get; set; }
        [Required]
        public int AccessFailedCount { get; set; }
    }
}
