using System;

namespace Festify.Promotion.DataAccess.Entities
{
    public class ShowRemoved
    {
        public int ShowRemovedId { get; set; }

        public int ShowId { get; set; }
        public Show Show { get; set; }

        public DateTime RemovedDate { get; set; }
    }
}