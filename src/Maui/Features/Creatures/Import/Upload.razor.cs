using AutoMapper;
using DMPowerTools.Core.Features.Combat;
using DMPowerTools.Core.Infrastructure;
using DMPowerTools.Core.Models;
using DMPowerTools.Core.Models.Imports;
using Microsoft.EntityFrameworkCore;
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
          
            ImportState.InReviewCreatures.Add( tetraCubeCreature.ConvertTetraCubeCreatureToCreature(tetraCubeCreature));
        }

        ImportState.TransitionStatus();
    }

}