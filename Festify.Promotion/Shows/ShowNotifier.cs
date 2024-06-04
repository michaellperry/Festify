using Festify.Promotion.Acts;
using Festify.Promotion.Data;
using Festify.Promotion.Messages.Acts;
using Festify.Promotion.Messages.Shows;
using Festify.Promotion.Messages.Venues;
using Festify.Promotion.Venues;
using MassTransit;

namespace Festify.Promotion.Shows;

public class ShowNotifier : INotifier<Show>
{
    private readonly ActQueries actQueries;
    private readonly VenueQueries venueQueries;
    private readonly IPublishEndpoint publishEndpoint;

    public ShowNotifier(ActQueries actQueries, VenueQueries venueQueries, IPublishEndpoint publishEndpoint)
    {
            this.actQueries = actQueries;
            this.venueQueries = venueQueries;
            this.publishEndpoint = publishEndpoint;
        }

    public async Task Notify(Show show)
    {
            var act = await actQueries.GetAct(show.Act.ActGuid);
            var venue = await venueQueries.GetVenue(show.Venue.VenueGuid);
            var showAdded = new ShowAdded
            {
                act = new ActRepresentation
                {
                    actGuid = act.ActGuid,
                    description = new ActDescriptionRepresentation
                    {
                        title = act.Title,
                        imageHash = act.ImageHash,
                        modifiedDate = new DateTime(act.LastModifiedTicks)
                    }
                },
                venue = new VenueRepresentation
                {
                    venueGuid = venue.VenueGuid,
                    description = new VenueDescriptionRepresentation
                    {
                        name = venue.Name,
                        city = venue.City,
                        modifiedDate = new DateTime(venue.LastModifiedTicks)
                    },
                    location = venue.ToVenueLocationRepresentation(),
                    timeZone = new VenueTimeZoneRepresentation
                    {
                        timeZone = venue.TimeZone,
                        modifiedDate = new DateTime(venue.TimeZoneLastModifiedTicks)
                    }
                },
                show = new ShowRepresentation
                {
                    startTime = show.StartTime
                }
            };

            await publishEndpoint.Publish(showAdded);
        }
}