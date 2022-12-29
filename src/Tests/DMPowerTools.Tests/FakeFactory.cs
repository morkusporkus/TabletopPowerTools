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

    public static string RandomString => AutoFaker.Generate<string>();
    public static int RandomNumber => AutoFaker.Generate<int>();
}
