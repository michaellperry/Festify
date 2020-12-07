using System;
using Festify.Promotion.Messages.Acts;
using Festify.Promotion.Messages.Venues;

namespace Festify.Promotion.Messages.Shows
{
    public class ShowAdded
    {
        public ActRepresentation act { get; set; }
        public VenueRepresentation venue { get; set; }
        public ShowRepresentation show { get; set; }
    }
}
