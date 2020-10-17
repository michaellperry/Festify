using System;
using System.ComponentModel.DataAnnotations;

namespace Festify.Promotion.DataAccess.Entities
{
    public class ShowDescription
    {
        public int ShowDescriptionId { get; set; }

        public Show Show { get; set; }
        public int ShowId { get; set; }
        public DateTime ModifiedDate { get; set; }

        [MaxLength(100)]
        public string Title { get; set; }
        public DateTime Date { get; set; }
        [MaxLength(50)]
        public string City { get; set; }
        [MaxLength(50)]
        public string Venue { get; set; }
        [MaxLength(88)]
        public string ImageHash { get; set; }
    }
}