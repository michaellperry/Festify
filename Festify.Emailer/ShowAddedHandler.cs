using Festify.Promotion.Messages.Shows;
using System;
using System.Threading.Tasks;

namespace Festify.Emailer
{
    class ShowAddedHandler
    {
        public ShowAddedHandler()
        {
        }

        public async Task Handle(ShowAdded message)
        {
            Console.WriteLine($"Emailing about a show for {message.act.description.title} at {message.venue.description.name}.");
            for (int tick = 0; tick < 45;  tick++)
            {
                await Task.Delay(60000);
                Console.WriteLine("[{0:s}] Waited {1} minutes", DateTime.Now, tick+1);
            }
        }
    }
}