using System;
using System.ComponentModel.DataAnnotations;

namespace Festify.Promotion.Venues
{
    public class VenueDescription
    {
        public int VenueDescriptionId { get; set; }

        public Venue Venue { get; set; }
        public int VenueId { get; set; }
        public DateTime ModifiedDate { get; set; }

        [MaxLength(50)]
        [Required]
        public string Name { get; set; }
        [MaxLength(50)]
        [Required]
        public string City { get; set; }
    }
}