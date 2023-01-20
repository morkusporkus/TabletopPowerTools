using DMPowerTools.Core.Features.Combat.Manage;

namespace DMPowerTools.Tests.Features.Combat.Manage;
public class CreatureSearchTests : IntegrationTestBase
{
    [Fact]
    public async Task GivenAnyCreaturesExist_ThenReturnsThem()
    {
        var mockCreature = CreateFakeCreature();
        await AddAsync(mockCreature);

        var result = await SendAsync(new CreatureSearchQuery());

        result.Creatures.Should().HaveCount(1);
    }
}
