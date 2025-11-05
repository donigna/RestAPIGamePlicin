using System;
using System.ComponentModel.DataAnnotations;

namespace Backend_Plicin.Models
{
    public class Player
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string PlayerName { get; set; } = string.Empty;

        [Required]
        public float Uang { get; set; }

        [Required]
        public int Level { get; set; }

        [Required]
        public int GameCount { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
