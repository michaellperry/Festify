using Festify.Promotion.DataAccess;
using Festify.Promotion.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Festify.Promotion.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ShowQueries showQueries;

        public IndexModel(ShowQueries showQueries)
        {
            this.showQueries = showQueries;
        }

        public List<ShowModel> Shows { get; set; }

        public async Task OnGetAsync()
        {
            Shows = await showQueries.ListShows();
        }
    }
}
