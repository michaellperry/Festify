using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Festify.Promotion.Projections;
using Festify.Promotion.Services.Entities;
using Microsoft.EntityFrameworkCore;

namespace Festify.Promotion.Services
{
    public class ShowQueries
    {
        private readonly PromotionContext repository;

        public ShowQueries(PromotionContext repository)
        {
            this.repository = repository;
        }

        public async Task<List<ShowProjection>> GetAllShows()
        {
            return await repository.Shows
                .Select(show => new ShowProjection
                {
                    ShowGuid = show.ShowGuid
                })
                .ToListAsync();
        }
    }
}