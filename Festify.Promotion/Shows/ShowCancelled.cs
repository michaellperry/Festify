using System;

namespace Festify.Promotion.Shows
{
    public class ShowCancelled
    {
        public int ShowCancelledId { get; set; }

        public Show Show { get; set; }
        public int ShowId { get; set; }
        public DateTime CancelledDate { get; set; }
    }
}
