using System;
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
        public IActionResult Handle()
        {
            try
            {
                return Ok(showsService.GetAllShows());
            }
            catch (Exception ex)
            {
                return this.Problem(ex.Message);
            }
        }
    }
}