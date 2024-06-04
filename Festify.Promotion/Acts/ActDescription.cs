using System.ComponentModel.DataAnnotations;

namespace Festify.Promotion.Acts;

public class ActDescription
{
    public int ActDescriptionId { get; set; }

    public Act Act { get; set; }
    public int ActId { get; set; }
    public DateTime ModifiedDate { get; set; }

    [MaxLength(100)]
    [Required]
    public string Title { get; set; }
    [MaxLength(88)]
    public string ImageHash { get; set; }
}