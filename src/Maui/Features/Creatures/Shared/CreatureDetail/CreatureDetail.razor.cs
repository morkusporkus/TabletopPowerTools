using DMPowerTools.Core.Features.Creatures.Shared;
using DMPowerTools.Core.Models;
using Markdig;

namespace DMPowerTools.Maui.Features.Creatures.Shared.CreatureDetail;
public partial class CreatureDetail
{
    [Inject] private IMediator Mediator { get; set; } = null!;

    [Parameter] public int CreatureId { get; set; }
    [Parameter] public Creature Creature { get; set; }

    private CreatureDetailQueryResponse _response;

    protected override async Task OnParametersSetAsync()
    {
        if (Creature is null) _response = await Mediator.Send(new CreatureDetailQuery { Id = CreatureId });
        else _response = new() { Creature = Creature };
    }

    public static MarkupString MarkdownDisplay(string item)
    {
        return (MarkupString)Markdown.ToHtml(item);
    }
}
