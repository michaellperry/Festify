﻿using Festify.Indexer.Documents;
using Festify.Indexer.Updaters;
using Festify.Promotion.Messages.Venues;
using System;
using System.Threading.Tasks;

namespace Festify.Indexer.Handlers;

public class VenueLocationChangedHandler
{
    private readonly IRepository repository;
    private readonly VenueUpdater venueUpdater;

    public VenueLocationChangedHandler(IRepository repository, VenueUpdater venueUpdater)
    {
            this.repository = repository;
            this.venueUpdater = venueUpdater;
        }

    public async Task Handle(VenueLocationChanged venueLocationChanged)
    {
            Console.WriteLine($"Updating index for venue location {venueLocationChanged.location.latitude}, {venueLocationChanged.location.longitude}.");
            try
            {
                string venueGuid = venueLocationChanged.venueGuid.ToString().ToLower();
                VenueLocation venueLocation = VenueLocation.FromRepresentation(venueLocationChanged.location);
                VenueDocument updatedVenue = new VenueDocument
                {
                    VenueGuid = venueGuid,
                    Location = venueLocation
                };
                VenueDocument venue = await venueUpdater.UpdateAndGetLatestVenue(updatedVenue);
                await repository.UpdateShowsWithVenueLocation(venue.VenueGuid, venue.Location);
                Console.WriteLine("Succeeded");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
}