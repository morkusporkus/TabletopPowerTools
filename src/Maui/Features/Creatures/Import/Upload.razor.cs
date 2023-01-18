using DMPowerTools.Core.Features.Creatures;

namespace DMPowerTools.Maui.Features.Creatures.Import;

public partial class Upload
{
    [Inject] private IMediator Mediator { get; set; } = null!;
    [CascadingParameter] public ImportState ImportState { get; set; }

    private async Task UploadFilesAsync(IReadOnlyList<IBrowserFile> files)
    {
        var response = await Mediator.Send(new ProcessMonsterFileCommand(files));

        ImportState.InReviewCreatures.AddRange(response.Creatures);

        ImportState.TransitionStatus();
    }

}