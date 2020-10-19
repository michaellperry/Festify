using System;
using System.Threading.Tasks;
using Festify.Promotion.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Festify.Promotion.Pages
{
    public class ShowRemoveModel : PageModel
    {
        private readonly ShowQueries showQueries;

        private readonly ShowCommands showCommands;

        public ShowRemoveModel(ShowQueries showQueries, ShowCommands showCommands)
        {
            this.showQueries = showQueries;
            this.showCommands = showCommands;
        }

        [BindProperty(SupportsGet=true)]
        public Guid ShowGuid { get; set; }

        public string Title { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public string City { get; set; }
        public string Venue { get; set; }
        public string ImageHash { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var show = await showQueries.GetShow(ShowGuid);

            if (show == null)
            {
                return NotFound();
            }
            else
            {
                if (show.Description != null)
                {
                    Title = show.Description.Title;
                    Date = show.Description.Date.ToLocalTime();
                    City = show.Description.City;
                    Venue = show.Description.Venue;
                    ImageHash = show.Description.ImageHash;
                }
                return Page();
            }
        }

        public async Task<IActionResult> OnPost()
        {
            await showCommands.RemoveShow(ShowGuid);
            return Redirect("~/");
        }
    }
}
