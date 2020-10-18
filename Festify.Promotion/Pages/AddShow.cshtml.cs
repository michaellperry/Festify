using Festify.Promotion.DataAccess;
using Festify.Promotion.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Threading.Tasks;

namespace Festify.Promotion.Pages
{
    public class AddShowModel : PageModel
    {
        private readonly ShowCommands commands;

        public AddShowModel(ShowCommands commands)
        {
            this.commands = commands;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost(string title, string date, string city, string venue)
        {
            await commands.SetShowDescription(Guid.NewGuid(), new ShowDescriptionModel
            {
                Title = title,
                Date = DateTime.Parse(date).ToUniversalTime(),
                City = city,
                Venue = venue,
                ImageHash = "what"
            });

            return Redirect("~/");
        }
    }
}
