using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Festify.Promotion.Venues
{
    public class Venue
    {
        public int VenueId { get; set; }
        [Required]
        public Guid VenueGuid { get; set; }

        public ICollection<VenueDescription> Descriptions { get; set; } = new List<VenueDescription>();
        public ICollection<VenueLocation> Locations { get; set; } = new List<VenueLocation>();
        public ICollection<VenueTimeZone> TimeZones { get; set; } = new List<VenueTimeZone>();
        public ICollection<VenueRemoved> Removed { get; set; } = new List<VenueRemoved>();
    }
}