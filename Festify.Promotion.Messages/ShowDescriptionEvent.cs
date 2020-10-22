using System;

namespace Festify.Promotion.Messages
{
  public class ShowDescriptionEvent
    {
        public Guid ShowGuid { get; set; }
        public DateTime ModifiedDate { get; set; }
        
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public string City { get; set; }
        public string Venue { get; set; }
        public string ImageHash { get; set; }
    }
}
