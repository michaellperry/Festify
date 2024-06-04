namespace Festify.Promotion.Venues;

public class VenueLocation
{
    public int VenueLocationId { get; set; }

    public int VenueId { get; set; }
    public Venue Venue { get; set; }

    public float Latitude { get; set; }
    public float Longitude { get; set; }

    public DateTime ModifiedDate { get; set; }
}