using System;
using System.Threading.Tasks;
using Festify.Promotion.DataAccess;
using Microsoft.AspNetCore.Mvc;

namespace Festify.Promotion.Controllers
{
    [ApiController]
    public class GetShows : Controller
    {
        private ShowQueries showQueries;

        public GetShows(ShowQueries showsService)
        {
            this.showQueries = showsService;
        }

        [HttpGet]
        [Route("shows")]
        public async Task<IActionResult> Handle()
        {
            try
            {
                var result = await showQueries.ListShows();
                return base.Ok(result);
            }
            catch (Exception ex)
            {
                return this.Problem(ex.Message);
            }
        }
    }
}