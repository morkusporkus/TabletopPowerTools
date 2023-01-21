using TabletopPowerTools.Core.Features.Creatures.Import;

namespace TabletopPowerTools.Maui.Features.Creatures.Import;

public partial class Upload
{
    [Inject] private IMediator Mediator { get; set; } = null!;
    [CascadingParameter] public ImportState ImportState { get; set; }

    private async Task UploadFilesAsync(IReadOnlyList<IBrowserFile> files, UploadSource uploadSource)
    {
        var response = await Mediator.Send(new UploadMonsterFileCommand(files, uploadSource));

        ImportState.InReviewCreatures.AddRange(response.Creatures);

        ImportState.TransitionStatus();
    }

}