using Markdig;

namespace DMPowerTools.Maui.Features.Creatures.Shared.CreatureDetail;
public partial class CreatureDetail
{
    [Parameter, EditorRequired] public Creature Creature { get; set; }
    public MarkupString MarkdownDisplay(string item)
    {
        return (MarkupString)Markdown.ToHtml(item);
    }
}
