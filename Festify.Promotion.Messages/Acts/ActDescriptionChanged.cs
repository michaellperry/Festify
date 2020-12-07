using System;

namespace Festify.Promotion.Messages.Acts
{
    public class ActDescriptionChanged
    {
        public Guid actGuid { get; set; }
        public ActDescriptionRepresentation description { get; set; }
    }
}
