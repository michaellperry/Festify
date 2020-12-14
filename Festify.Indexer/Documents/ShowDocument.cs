using System;

namespace Festify.Indexer.Documents
{
    public class ShowDocument
    {
        // Hash of the alternate key
        public string Id { get; set; }

        // Alternate key
        public string ActGuid { get; set; }
        public string VenueGuid { get; set; }
        public DateTimeOffset StartTime { get; set; }

        // Content
        public ActDescription ActDescription { get; set; }
        public VenueDescription VenueDescription { get; set; }
        public VenueLocation VenueLocation { get; set; }
    }
}
