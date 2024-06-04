using FluentAssertions;
using Festify.Promotion.Acts;
using Festify.Promotion.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Festify.Promotion.UnitTest;

public class ActTests
{
    [Fact]
    public async Task ActsInitiallyEmpty()
    {
        List<ActInfo> acts = await actQueries.ListActs();
        acts.Should().BeEmpty();
    }

    [Fact]
    public async Task WhenAddAct_ActIsReturned()
    {
        var actGuid = Guid.NewGuid();
        await actCommands.SaveAct(ActModelWith(actGuid, "Gabriel Iglesias"));

        var acts = await actQueries.ListActs();
        acts.Should().Contain(act => act.ActGuid == actGuid);
    }

    [Fact]
    public async Task WhenAddActTwice_OneActIsAdded()
    {
        var actGuid = Guid.NewGuid();
        await actCommands.SaveAct(ActModelWith(actGuid, "Gabriel Iglesias"));
        await actCommands.SaveAct(ActModelWith(actGuid, "Gabriel Iglesias"));

        var acts = await actQueries.ListActs();
        acts.Count.Should().Be(1);
    }

    [Fact]
    public async Task WhenSetActDescription_ActDescriptionIsReturned()
    {
        var actGuid = Guid.NewGuid();
        await actCommands.SaveAct(ActModelWith(actGuid, "Gabriel Iglesias"));

        var act = await actQueries.GetAct(actGuid);
        act.Title.Should().Be("Gabriel Iglesias");
    }

    [Fact]
    public async Task WhenChangeActDescription_ActDescriptionIsModified()
    {
        var actGuid = Guid.NewGuid();
        await actCommands.SaveAct(ActModelWith(actGuid, "Gabriel Iglesias"));
        var versionOne = await actQueries.GetAct(actGuid);
        await actCommands.SaveAct(ActModelWith(actGuid, "Jeff Dunham", versionOne.LastModifiedTicks));

        var act = await actQueries.GetAct(actGuid);
        act.Title.Should().Be("Jeff Dunham");
    }

    [Fact]
    public async Task WhenActDescriptionIsTheSame_ActDescriptionIsNotModified()
    {
        var actGuid = Guid.NewGuid();
        await actCommands.SaveAct(ActModelWith(actGuid, "Gabriel Iglesias"));
        var versionOne = await actQueries.GetAct(actGuid);
        await actCommands.SaveAct(ActModelWith(actGuid, "Gabriel Iglesias", versionOne.LastModifiedTicks));

        var act = await actQueries.GetAct(actGuid);
        act.LastModifiedTicks.Should().Be(versionOne.LastModifiedTicks);
    }

    [Fact]
    public async Task WhenBasedOnOldVersion_ChangeIsRejected()
    {
        var actGuid = Guid.NewGuid();
        await actCommands.SaveAct(ActModelWith(actGuid, "Gabriel Iglesias"));
        var versionOne = await actQueries.GetAct(actGuid);
        await actCommands.SaveAct(ActModelWith(actGuid, "Jeff Dunham", versionOne.LastModifiedTicks));

        Func<Task> update = async () =>
        {
            await actCommands.SaveAct(ActModelWith(actGuid, "Jeff Foxworthy", versionOne.LastModifiedTicks));
        };
        await update.Should().ThrowAsync<DbUpdateConcurrencyException>();
    }

    [Fact]
    public async Task GivenActDoesNotExist_WhenSetActDescription_ActIsCreated()
    {
        var actGuid = Guid.NewGuid();
        await actCommands.SaveAct(ActModelWith(actGuid, "Gabriel Iglesias"));

        var act = await actQueries.GetAct(actGuid);
        act.Title.Should().Be("Gabriel Iglesias");
    }

    [Fact]
    public async Task WhenRemoveAct_ActIsNotReturned()
    {
        var actGuid = Guid.NewGuid();
        await actCommands.SaveAct(ActModelWith(actGuid, "Gabriel Iglesias"));
        await actCommands.RemoveAct(actGuid);

        var acts = await actQueries.ListActs();
        acts.Should().BeEmpty();
    }

    private ActInfo ActModelWith(Guid actGuid, string title, long lastModifiedTicks = 0)
    {
        var sha512 = SHA512.Create();
        var imageHash = sha512.ComputeHash(Encoding.UTF8.GetBytes(title));

        var actModel = new ActInfo
        {
            ActGuid = actGuid,
            Title = title,
            ImageHash = Convert.ToBase64String(imageHash),
            LastModifiedTicks = lastModifiedTicks
        };
        return actModel;
    }

    private ActQueries actQueries;
    private ActCommands actCommands;

    public ActTests()
    {
        var repository = new PromotionContext(new DbContextOptionsBuilder<PromotionContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options, null);

        actQueries = new ActQueries(repository);
        actCommands = new ActCommands(repository);
    }
}