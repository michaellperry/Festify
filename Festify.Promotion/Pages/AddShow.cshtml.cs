using Festify.Promotion.DataAccess;
using Festify.Promotion.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Threading.Tasks;

namespace Festify.Promotion.Pages
{
    public class AddShowModel : PageModel
    {
        private readonly ShowQueries queries;
        private readonly ShowCommands commands;

        public AddShowModel(ShowQueries queries, ShowCommands commands)
        {
            this.queries = queries;
            this.commands = commands;
        }

        [BindProperty(SupportsGet=true)]
        public Guid ShowGuid { get; set; }

        [BindProperty]
        public string Title { get; set; }
        [BindProperty]
        public DateTime Date { get; set; }
        [BindProperty]
        public string City { get; set; }
        [BindProperty]
        public string Venue { get; set; }
        [BindProperty]
        public IFormFile Image { get; set; }

        public async Task OnGet()
        {
            var show = await queries.GetShow(ShowGuid);

            if (show == null)
            {

            }
            else
            {
                Title = show.Description.Title;
                Date = show.Description.Date.ToLocalTime();
                City = show.Description.City;
                Venue = show.Description.Venue;
            }
        }

        public async Task<IActionResult> OnPost()
        {
            await commands.SetShowDescription(ShowGuid, new ShowDescriptionModel
            {
                Title = Title,
                Date = Date.ToUniversalTime(),
                City = City,
                Venue = Venue,
                ImageHash = "what"
            });

            return Redirect("~/");
        }
    }
}
