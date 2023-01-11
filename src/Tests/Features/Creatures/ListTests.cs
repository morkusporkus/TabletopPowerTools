using DMPowerTools.Core.Features.Creatures;
using DMPowerTools.Core.Models;

namespace DMPowerTools.Tests.Features.Creatures;

public class ListTests : IntegrationTestBase
{
    [Fact]
    public async Task GivenAnyCreaturesExist_ThenReturnsThem()
    {
        var mockCreature = CreateFakeCreature();
        await AddAsync(mockCreature);

        var result = await SendAsync(new ListQuery());

        result.Creatures.Should().HaveCount(1);
    }
}

public class DeleteTests : IntegrationTestBase
{
    [Fact]
    public async Task GivenAnyExistingCreature_ThenIsDeleted()
    {
        var mockCreature = CreateFakeCreature();
        await AddAsync(mockCreature);

        await SendAsync(new DeleteCommand { Id = mockCreature.Id });

        var creature = await FirstOrDefaultAsync<Creature>();
        creature.Should().BeNull();
    }
}
