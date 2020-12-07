using Festify.Promotion.Messages.Shows;
using System;
using System.Threading.Tasks;

namespace Festify.Indexer
{
    public class ShowAddedHandler
    {
        private readonly IRepository repository;

        public ShowAddedHandler(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task Handle(ShowAdded showAdded)
        {
            Console.WriteLine($"Indexing a show for {showAdded.act.description.title} at {showAdded.venue.description.name}.");
            try
            {
                await repository.IndexShow(showAdded);
                Console.WriteLine("Succeeded");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}