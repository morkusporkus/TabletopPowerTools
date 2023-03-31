using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopPowerTools.Core.Features.Creatures.Import;
using TabletopPowerTools.Core.Infrastructure;

namespace TabletopPowerTools.Tests.Features.Creatures
{
    public  class Create : IntegrationTestBase
    {
        [Fact]
        public async Task GivenAnyCreature_WhenItIsFinalized_ThenIsSaved()
        {
            var mockCreature = CreateFakeCreatureViewModel();
            var command = new CreateCreatureCommand{Creature=mockCreature };
            await SendAsync(command);

            var dbContext = GetRequiredService<ApplicationDbContext>();
            dbContext.Creatures.Should().HaveCount(1);
        }
    }
}
