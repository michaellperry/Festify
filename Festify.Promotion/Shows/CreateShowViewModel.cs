using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Festify.Promotion.Shows;

public class CreateShowViewModel
{
    public ActInfo Act { get; set; }
    public List<SelectListItem> Venues { get; set; }
    public Guid Venue { get; set; }
    public DateTime StartTime { get; set; }
}