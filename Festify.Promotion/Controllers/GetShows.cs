using System;
using System.Threading.Tasks;
using Festify.Promotion.Services;
using Microsoft.AspNetCore.Mvc;

namespace Festify.Promotion.Controllers
{
    [ApiController]
    public class GetShows : Controller
    {
        private ShowsService showsService;

        public GetShows(ShowsService showsService)
        {
            this.showsService = showsService;
        }

        [HttpGet]
        [Route("shows")]
        public async Task<IActionResult> Handle()
        {
            try
            {
                var result = await showsService.GetAllShows();
                return base.Ok(result);
            }
            catch (Exception ex)
            {
                return this.Problem(ex.Message);
            }
        }
    }
}