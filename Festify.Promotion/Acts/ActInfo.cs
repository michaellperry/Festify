using Microsoft.AspNetCore.Http;
using System;

namespace Festify.Promotion.Acts
{
    public class ActInfo
    {
        public Guid ActGuid { get; set; }
        public string Title { get; set; }
        public IFormFile Image { get; set; }
        public string ImageHash { get; set; }
        public long LastModifiedTicks { get; set; }
    }
}