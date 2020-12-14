using Festify.Promotion.Messages.Acts;
using System;

namespace Festify.Indexer.Documents
{
    public class ActDescription
    {
        public string Title { get; set; }
        public string ImageHash { get; set; }
        public DateTime ModifiedDate { get; set; }

        public static ActDescription FromRepresentation(ActDescriptionRepresentation description)
        {
            return new ActDescription
            {
                Title = description.title,
                ImageHash = description.imageHash,
                ModifiedDate = description.modifiedDate
            };
        }
    }
}