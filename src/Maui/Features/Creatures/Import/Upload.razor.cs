using AutoMapper;
using DMPowerTools.Core.Models;
using DMPowerTools.Core.Models.Imports;
using System.Text.Json;

namespace DMPowerTools.Maui.Features.Creatures.Import;

public partial class Upload
{
    [CascadingParameter] public ImportState ImportState { get; set; }

    private async Task UploadFilesAsync(IReadOnlyList<IBrowserFile> files)
    {
        foreach (var file in files)
        {
            var tetraCubeCreature = await JsonSerializer.DeserializeAsync<TetraCubeCreature>(file.OpenReadStream(512000, default), new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            var config = new MapperConfiguration(cfg => cfg.CreateMap<TetraCubeCreature, Creature>());
            var mapper = config.CreateMapper();
            var creature = mapper.Map<Creature>(tetraCubeCreature);
            creature.AC = creature.CalculateACFromTetraCube(tetraCubeCreature.ArmorName,tetraCubeCreature.DexPoints,tetraCubeCreature.NatArmorBonus);
            ImportState.InReviewCreatures.Add(creature);
        }

        ImportState.TransitionStatus();
    }
}
