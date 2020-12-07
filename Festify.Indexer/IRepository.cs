using Festify.Promotion.Messages.Acts;
using Festify.Promotion.Messages.Shows;
using System;
using System.Threading.Tasks;

namespace Festify.Indexer
{
    public interface IRepository
    {
        Task IndexShow(ShowAdded showAdded);
        Task UpdateShowsWithActDescription(Guid actGuid, ActDescriptionRepresentation description);
    }
}