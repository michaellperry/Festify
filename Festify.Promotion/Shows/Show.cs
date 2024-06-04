using Festify.Promotion.Venues;
using System.Collections.Generic;
using Festify.Promotion.Acts;

namespace Festify.Promotion.Shows;

public class Show
{
    public int ShowId { get; set; }

    public Act Act { get; set; }
    public int ActId { get; set; }
    public Venue Venue { get; set; }
    public int VenueId { get; set; }
    public DateTimeOffset StartTime { get; set; }

    public ICollection<ShowCancelled> Cancelled { get; set; } = new List<ShowCancelled>();
}