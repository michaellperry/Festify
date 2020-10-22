using System;

namespace Festify.Promotion.Messages
{
  public class ShowRemovedEvent
    {
        public Guid ShowGuid { get; set; }
        public DateTime RemovedDate { get; set; }
    }
}
