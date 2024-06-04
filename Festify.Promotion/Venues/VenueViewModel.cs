using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Festify.Promotion.Venues;

public class VenueViewModel
{
    public VenueInfo Venue { get; set; }
    public List<SelectListItem> TimeZones { get; set; }
}