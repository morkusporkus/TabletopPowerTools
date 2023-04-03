using Bogus.DataSets;
using TabletopPowerTools.Core.Features.Creatures;
using TabletopPowerTools.Core.Infrastructure;
using TabletopPowerTools.Core.Models.ViewModels;

namespace TabletopPowerTools.Tests.Features.Creatures
{
    public class Create : IntegrationTestBase
    {
        [Fact]
        public async Task GivenAnyCreature_WhenItIsFinalized_ThenIsSaved()
        {
            var mockCreature = CreateFakeCreatureViewModel();
            var fakeAbility = new AutoFaker<AbilityViewModel>().Generate();
            var fakeAction = new AutoFaker<ActionViewModel>().Generate();
            mockCreature.Abilities.Add(fakeAbility);
            mockCreature.Actions.Add(fakeAction);

            var command = new CreateCreatureCommand { Creature = mockCreature };
            await SendAsync(command);

            var dbContext = GetRequiredService<ApplicationDbContext>();
            dbContext.Creatures.Should().HaveCount(1);
            dbContext.Actions.Should().HaveCount(1);
            dbContext.Actions.ElementAt(0).Should().NotBeNull();
        }
    }
}
