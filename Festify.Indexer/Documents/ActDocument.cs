namespace Festify.Indexer.Documents;

public class ActDocument
{
    // Hash of the alternate key
    public string Id { get; set; }

    // Alternate key
    public string ActGuid { get; set; }

    // Content
    public ActDescription Description { get; set; }
}