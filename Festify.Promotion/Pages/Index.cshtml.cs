using Festify.Promotion.Acts;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Festify.Promotion.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ActQueries actQueries;

        public IndexModel(ActQueries actQueries)
        {
            this.actQueries = actQueries;
        }

        public List<ActInfo> Acts { get; set; }

        public async Task OnGetAsync()
        {
            Acts = await actQueries.ListActs();
        }
    }
}
