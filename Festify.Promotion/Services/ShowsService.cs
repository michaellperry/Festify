using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Festify.Promotion.Projections;

namespace Festify.Promotion.Services
{
    public class ShowsService
    {
        public async Task<List<ShowProjection>> GetAllShows()
        {
            return new List<ShowProjection>();
        }
    }
}