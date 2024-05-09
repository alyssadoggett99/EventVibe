using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventVibe.Models
{
    public class Registration
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RegistrationId { get; set; }

        public int EventId { get; set; }
        public int UserId { get; set; }

        [Required]
        public DateTime DateRegistered { get; set; }

        [Required]
        public string RegistrationStatus { get; set; }

        // Navigation properties
        public Event Event { get; set; }
        public ApplicationUser User { get; set; }  
    }
}
