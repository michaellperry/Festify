using Festify.Promotion.Contents;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace Festify.Promotion.Pages.Acts;

public class IndexModel : PageModel
{
    private readonly ActQueries actQueries;
    private readonly ActCommands actCommands;
    private readonly ContentQueries contentQueries;
    private readonly ContentCommands contentCommands;

    public IndexModel(ActQueries actQueries, ActCommands actCommands, ContentQueries contentQueries, ContentCommands contentCommands)
    {
            this.actQueries = actQueries;
            this.actCommands = actCommands;
            this.contentQueries = contentQueries;
            this.contentCommands = contentCommands;
        }

    [BindProperty(SupportsGet = true)]
    public Guid ActGuid { get; set; }

    public bool AddAct { get; set; }

    [BindProperty]
    public string Title { get; set; }
    [BindProperty]
    public IFormFile Image { get; set; }
    [BindProperty]
    public string ImageHash { get; set; }
    [BindProperty]
    public long LastModifiedTicks { get; set; }
    public string ErrorMessage { get; set; }

    public async Task OnGet()
    {
            var act = await actQueries.GetAct(ActGuid);

            if (act == null)
            {
                AddAct = true;
            }
            else
            {
                AddAct = false;
                Title = act.Title;
                ImageHash = act.ImageHash;
                LastModifiedTicks = act.LastModifiedTicks;
            }
        }

    public async Task<IActionResult> OnPost()
    {
            var imageHash = await GetImageHash();

            try
            {
                await actCommands.SaveAct(new ActInfo
                {
                    ActGuid = ActGuid,
                    Title = Title,
                    ImageHash = imageHash,
                    LastModifiedTicks = LastModifiedTicks
                });
            }
            catch (DbUpdateConcurrencyException ex)
            {
                ErrorMessage = ex.Message;
                return Page();
            }

            return Redirect("~/");
        }

    private async Task<string> GetImageHash()
    {
            if (Image != null)
            {
                using var imageReadStream = Image.OpenReadStream();
                using var imageMemoryStream = new MemoryStream();
                await imageReadStream.CopyToAsync(imageMemoryStream);
                var imageHash = await contentCommands.SaveContent(imageMemoryStream.ToArray(), Image.ContentType);
                return imageHash;
            }
            else
            {
                return ImageHash;
            }
        }
}