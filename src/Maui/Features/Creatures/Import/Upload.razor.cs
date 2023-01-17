using DMPowerTools.Core.Models;
using System.Text.Json;

namespace DMPowerTools.Maui.Features.Creatures.Import;

public partial class Upload
{
    [CascadingParameter] public ImportState ImportState { get; set; }

    private async Task UploadFilesAsync(IReadOnlyList<IBrowserFile> files, ImportSource importSource)
    {
        foreach (var file in files)
        {
            var creature = importSource switch
            {
                ImportSource.TetraCube or ImportSource.TableTopPowerTools => await JsonSerializer.DeserializeAsync<Creature>(file.OpenReadStream(512000, default), new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
            };

            ImportState.InReviewCreatures.Add(creature);
        }

        ImportState.TransitionStatus();
    }
}
