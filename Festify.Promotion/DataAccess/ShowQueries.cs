using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Festify.Promotion.DataAccess.Entities;
using Festify.Promotion.Projections;
using Microsoft.EntityFrameworkCore;

namespace Festify.Promotion.DataAccess
{
    public class ShowQueries
    {
        private readonly PromotionContext repository;

        public ShowQueries(PromotionContext repository)
        {
            this.repository = repository;
        }

        public async Task<List<ShowProjection>> ListShows()
        {
            return await repository.Show
                .Select(show => new ShowProjection
                {
                    ShowGuid = show.ShowGuid
                })
                .ToListAsync();
        }
    }
}