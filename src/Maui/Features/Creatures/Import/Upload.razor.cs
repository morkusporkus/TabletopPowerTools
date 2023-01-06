using System.Text.Json;

namespace DMPowerTools.Maui.Features.Creatures.Import;

public partial class Upload
{
    [Inject] public ApplicationDbContext DbContext { get; set; }
    [CascadingParameter] public ImportState ImportState { get; set; }

    private async Task UploadFilesAsync(InputFileChangeEventArgs e)
    {
        // TODO: the way this is importing seems incorrect.
        foreach (IBrowserFile f in e.GetMultipleFiles())
        {
            var creature = await JsonSerializer.DeserializeAsync<Creature>(f.OpenReadStream(512000, default), new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            ImportState.InReviewCreatures.Add(creature);
        }

        ImportState.TransitionStatus();
    }
}
