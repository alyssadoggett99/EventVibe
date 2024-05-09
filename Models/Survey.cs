using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventVibe.Models
{
    public class Survey
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SurveyId { get; set; }
        public int EventId { get; set; }
        public int UserId { get; set; }
        
        public string CommentDetails { get; set; }
        
        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }

        // Navigation properties
        public Event Event { get; set; }
        public ApplicationUser User { get; set; } // Assuming ApplicationUser is used
    }
}
