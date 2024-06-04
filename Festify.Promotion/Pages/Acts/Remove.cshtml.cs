using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Festify.Promotion.Pages.Acts;

public class RemoveModel : PageModel
{
    private readonly ActQueries actQueries;

    private readonly ActCommands actCommands;

    public RemoveModel(ActQueries actQueries, ActCommands actCommands)
    {
        this.actQueries = actQueries;
        this.actCommands = actCommands;
    }

    [BindProperty(SupportsGet = true)] public Guid ActGuid { get; set; }

    public string Title { get; set; }
    public string ImageHash { get; set; }

    public async Task<IActionResult> OnGet()
    {
        var act = await actQueries.GetAct(ActGuid);

        if (act == null)
        {
            return NotFound();
        }
        else
        {
            Title = act.Title;
            ImageHash = act.ImageHash;
            return Page();
        }
    }

    public async Task<IActionResult> OnPost()
    {
        await actCommands.RemoveAct(ActGuid);
        return Redirect("~/");
    }
}