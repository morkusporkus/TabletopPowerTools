using DMPowerTools.Core.Features.Creatures.Import;
using DMPowerTools.Core.Models;

namespace DMPowerTools.Maui.Features.Creatures.Import;
public partial class Review
{
    [Inject] public ISnackbar Snackbar { get; set; }
    [Inject] public IMediator Mediator { get; set; }
    [CascadingParameter] public ImportState ImportState { get; set; }

    private Creature _inReviewCreature;
    private bool _isDuplicate;

    protected override async Task OnInitializedAsync()
    {
        await AssignNextCreatureAsync();
    }

    private async Task AcceptAsync()
    {
        await Mediator.Send(new AcceptCreatureCommand { Creature = _inReviewCreature });

        await AssignNextCreatureAsync();
    }

    private async Task DenyAsync() => await AssignNextCreatureAsync();

    private async Task AcceptAllAsync()
    {
        await Mediator.Send(new AcceptAllCreaturesCommand { Creatures = ImportState.InReviewCreatures });

        ImportState.TransitionStatus();
    }

    private async Task AssignNextCreatureAsync()
    {
        if (_inReviewCreature != null)
        {
            ImportState.InReviewCreatures.Remove(_inReviewCreature);
        }

        _inReviewCreature = ImportState.InReviewCreatures.FirstOrDefault();

        if (_inReviewCreature == null)
        {
            ImportState.TransitionStatus();
        }
        else
        {
            _isDuplicate = await Mediator.Send(new IsDuplicateCreatureQuery { Name = _inReviewCreature.Name });
        }
    }
}
