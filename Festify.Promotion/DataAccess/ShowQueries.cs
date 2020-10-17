using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Festify.Promotion.DataAccess.Entities;
using Festify.Promotion.Models;
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

        public async Task<List<ShowModel>> ListShows()
        {
            var result = await repository.Show
                .Select(show => new
                {
                    Show = show,
                    Description = show.Descriptions
                        .OrderByDescending(d => d.ModifiedDate)
                        .FirstOrDefault()
                })
                .ToListAsync();

            return result
                .Select(row => new ShowModel
                {
                    ShowGuid = row.Show.ShowGuid,
                    Description = row.Description == null ? null : new ShowDescriptionModel
                    {
                        Title = row.Description.Title,
                        Date = row.Description.Date,
                        City = row.Description.City,
                        Venue = row.Description.Venue,
                        ImageHash = row.Description.ImageHash
                    }
                })
                .ToList();
        }
    }
}