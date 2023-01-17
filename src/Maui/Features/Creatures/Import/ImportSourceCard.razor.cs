namespace DMPowerTools.Maui.Features.Creatures.Import;
public partial class ImportSourceCard : ComponentBase
{
    [Parameter, EditorRequired] public EventCallback<IReadOnlyList<IBrowserFile>> OnFilesChangedCallback { get; set; }
    [Parameter, EditorRequired] public string Title { get; set; }
    [Parameter, EditorRequired] public RenderFragment Body { get; set; }
    [Parameter, EditorRequired] public string ImagePath { get; set; }
}
