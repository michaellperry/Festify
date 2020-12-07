using System;

namespace Festify.Promotion.Messages.Venues
{
    public class VenueLocationChanged
    {
        public Guid venueGuid { get; set; }
        public VenueLocationRepresentation location { get; set; }
    }
}
