using DMPowerTools.Core.Models;
using DMPowerTools.Core.Models.Imports;

namespace DMPowerTools.Tests;

internal class FakeFactory
{
    public FakeFactory()
    {
        AutoFaker.Configure(builder =>
        {
            builder
              .WithRecursiveDepth(0)
              .WithTreeDepth(0);
        });
    }

    public static Creature CreateFakeCreature()
    {
        var faker = new AutoFaker<Creature>()
            .Ignore(c => c.Id)
            .Ignore(c => c.Abilities)
            .Ignore(c => c.Actions)
            .Ignore(c => c.Skills);

        return faker.Generate();
    }

    public static TetraCubeCreature CreateFakeTetraCubeCreature()
    {
        var faker = new AutoFaker<TetraCubeCreature>()
            .Ignore(c => c.Abilities)
            .Ignore(c => c.Actions)
            .Ignore(c => c.Skills);

        return faker.Generate();
    }

    public static string RandomString => AutoFaker.Generate<string>();
    public static int RandomNumber => AutoFaker.Generate<int>();
}
