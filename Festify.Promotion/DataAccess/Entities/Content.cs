using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Festify.Promotion.DataAccess.Entities
{
    public class Content
    {
        [MaxLength(88)]
        public string Hash { get; set; }
        public byte[] Binary { get; set; }
    }
}
