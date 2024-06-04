using Festify.Promotion.Messages.Shows;
using System;
using System.Threading.Tasks;

namespace Festify.Emailer;

class ShowAddedHandler
{
    public ShowAddedHandler()
    {
        }

    public Task Handle(ShowAdded message)
    {
            Console.WriteLine($"Emailing about a show for {message.act.description.title} at {message.venue.description.name}.");
            return Task.CompletedTask;
        }
}