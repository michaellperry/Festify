namespace Festify.Indexer.Documents
{
    public class VenueDocument
    {
        // Hash of the alternate key
        public string Id { get; set; }

        // Alternate key
        public string VenueGuid { get; set; }

        // Content
        public VenueDescription Description { get; set; }
        public VenueLocation Location { get; set; }
    }
}
