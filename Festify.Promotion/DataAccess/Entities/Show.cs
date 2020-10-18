using System;
using System.Collections;
using System.Collections.Generic;

namespace Festify.Promotion.DataAccess.Entities
{
    public class Show
    {
        public int ShowId { get; set; }
        public Guid ShowGuid { get; set; }

        public ICollection<ShowDescription> Descriptions { get; set; } = new List<ShowDescription>();
        public ICollection<ShowRemoved> Removed { get; set; } = new List<ShowRemoved>();
    }
}