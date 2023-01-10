using AutoMapper;
using DMPowerTools.Core.Infrastructure;

namespace DMPowerTools.Tests;
public class MappingTests
{
    [Fact]
    public void AllMappersMapToProperties()
    {
        var allProfiles = typeof(ApplicationDbContext).Assembly
            .GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(Profile)))
            .Select(type => (Profile)Activator.CreateInstance(type)!);

        var mapperConfiguration = new MapperConfiguration(_ => _.AddProfiles(allProfiles));

        mapperConfiguration.AssertConfigurationIsValid();
    }
}
