using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventVibe.Models
{
    public class Event
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EventId { get; set; }
        public int OrganizerId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Event name cannot be longer than 100 characters.")]
        public string EventName { get; set; }

        public string Description { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public ICollection<Survey> Surveys { get; set; }

        // Assuming ApplicationUser is used
        [ForeignKey("OrganizerId")]
        public ApplicationUser Organizer { get; set; }

        public ICollection<Registration> Registrations { get; set; }
    }
}
