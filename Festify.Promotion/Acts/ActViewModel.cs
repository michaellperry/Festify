using System.Collections.Generic;
using Festify.Promotion.Shows;

namespace Festify.Promotion.Acts;

public class ActViewModel
{
    public ActInfo Act { get; set; }
    public List<ShowInfo> Shows { get; set; }
}