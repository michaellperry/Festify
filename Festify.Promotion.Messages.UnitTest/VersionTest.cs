using System;
using System.Text.Json;
using Festify.Promotion.Messages.Venues;
using FluentAssertions;
using Xunit;

namespace Festify.Promotion.Messages.UnitTest;

public class VersionTest
{
    [Fact]
    public void CanDeserializeVenue_Version1()
    {
            string venueRepresentationStrV1 = @"{
    ""venueGuid"": ""E12AEEB5-9443-4AD4-B394-EFB37B0DD67F"",
    ""description"": {
        ""name"": ""Madison Square Garden"",
        ""city"": ""New York, NY"",
        ""modifiedDate"": ""2020-11-22T22:09:27.993Z""
    }
}";
            var venueRepresentation = JsonSerializer.Deserialize<VenueRepresentation>(venueRepresentationStrV1);
            venueRepresentation.venueGuid.Should().Be(Guid.Parse("E12AEEB5-9443-4AD4-B394-EFB37B0DD67F"));
            venueRepresentation.description.name.Should().Be("Madison Square Garden");
            venueRepresentation.description.city.Should().Be("New York, NY");
            venueRepresentation.description.modifiedDate.Should().Be(new DateTime(2020, 11, 22, 22, 9, 27, 993, DateTimeKind.Utc));

            venueRepresentation.location.Should().BeNull();
        }

    [Fact]
    public void CanDeserializeVenue_Version2()
    {
            string venueRepresentationStrV1 = @"{
    ""venueGuid"": ""E12AEEB5-9443-4AD4-B394-EFB37B0DD67F"",
    ""description"": {
        ""name"": ""Madison Square Garden"",
        ""city"": ""New York, NY"",
        ""modifiedDate"": ""2020-11-22T22:09:27.993Z""
    },
    ""location"": {
        ""latitude"": 40.7505,
        ""longitude"": -73.9934,
        ""modifiedDate"": ""2020-11-28T05:06:39.215Z""
    }
}";
            var venueRepresentation = JsonSerializer.Deserialize<VenueRepresentation>(venueRepresentationStrV1);
            venueRepresentation.venueGuid.Should().Be(Guid.Parse("E12AEEB5-9443-4AD4-B394-EFB37B0DD67F"));
            venueRepresentation.description.name.Should().Be("Madison Square Garden");
            venueRepresentation.description.city.Should().Be("New York, NY");
            venueRepresentation.description.modifiedDate.Should().Be(new DateTime(2020, 11, 22, 22, 9, 27, 993, DateTimeKind.Utc));

            venueRepresentation.location.latitude.Should().Be(40.7505f);
            venueRepresentation.location.longitude.Should().Be(-73.9934f);
            venueRepresentation.location.modifiedDate.Should().Be(new DateTime(2020, 11, 28, 5, 6, 39, 215, DateTimeKind.Utc));
        }
}