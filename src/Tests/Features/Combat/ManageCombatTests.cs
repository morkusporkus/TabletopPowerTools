using DMPowerTools.Core.Features.Combat;

namespace DMPowerTools.Tests.Features.Combat;
public class ManageCombatTests : IntegrationTestBase
{
    [Fact]
    public async Task GivenAnyCreaturesExist_ThenReturnsThem()
    {
        var mockCreature = CreateFakeCreature();
        await AddAsync(mockCreature);

        var result = await SendAsync(new ManageCombatQuery());

        result.Creatures.Should().HaveCount(1);
    }
}
