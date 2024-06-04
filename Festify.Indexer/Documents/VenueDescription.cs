using Festify.Promotion.Messages.Venues;
using System;

namespace Festify.Indexer.Documents;

public class VenueDescription
{
    public string Name { get; set; }
    public string City { get; set; }
    public DateTime ModifiedDate { get; set; }

    public static VenueDescription FromRepresentation(VenueDescriptionRepresentation description)
    {
            return new VenueDescription
            {
                Name = description.name,
                City = description.city,
                ModifiedDate = description.modifiedDate
            };
        }
}