using TabletopPowerTools.Core.Features.Creatures.Shared;
using TabletopPowerTools.Core.Models;

namespace TabletopPowerTools.Maui.Features.Creatures.Shared.CreatureDetail;
public partial class CreatureDetail
{
    [Inject] private IMediator Mediator { get; set; } = null!;

    [Parameter] public int CreatureId { get; set; }
    [Parameter] public Creature Creature { get; set; }
    [Parameter] public EventCallback<Creature> CreatureChanged { get; set; }
    [Parameter] public bool ReadOnly { get; set; } = true;

    protected override async Task OnInitializedAsync()
    {
        if (Creature is null)
        {
            var response = await Mediator.Send(new CreatureDetailQuery { Id = CreatureId });
            Creature = response.Creature;
        }
    }
}
