using System;

namespace Festify.Promotion.DataAccess.Entities
{
    public class ShowDescription
    {
        public int ShowDescriptionId { get; set; }

        public Show Show { get; set; }
        public int ShowId { get; set; }
        public DateTime ModifiedDate { get; set; }

        public string Title { get; set; }
        public DateTime Date { get; set; }
        public string City { get; set; }
        public string Venue { get; set; }
        public string ImageHash { get; set; }
    }
}