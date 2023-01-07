using System.Text.Json;

namespace DMPowerTools.Maui.Features.Creatures.Import;

public partial class Upload
{
    [Inject] public ApplicationDbContext DbContext { get; set; }
    [CascadingParameter] public ImportState ImportState { get; set; }

    private async Task UploadFilesAsync(IReadOnlyList<IBrowserFile> files)
    {
        foreach (var file in files)
        {
            var creature = await JsonSerializer.DeserializeAsync<Creature>(file.OpenReadStream(512000, default), new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            ImportState.InReviewCreatures.Add(creature);
        }

        ImportState.TransitionStatus();
    }
}
