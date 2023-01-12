using DMPowerTools.Core.Features.Creatures.Shared;
using DMPowerTools.Core.Models;

namespace DMPowerTools.Maui.Features.Creatures.Shared.CreatureDetail;
public partial class CreatureDetail
{
    [Inject] private IMediator Mediator { get; set; } = null!;

    [Parameter] public int CreatureId { get; set; }
    [Parameter] public Creature Creature { get; set; }
    [Parameter] public bool ReadOnly { get; set; } = true;

    private CreatureDetailQueryResponse _response;

    protected override async Task OnParametersSetAsync()
    {
        if (Creature is null) _response = await Mediator.Send(new CreatureDetailQuery { Id = CreatureId });
        else _response = new() { Creature = Creature };
    }
}
