using DMPowerTools.Core.Features.Combat.Manage;
using DMPowerTools.Core.Models;

namespace DMPowerTools.Maui.Features.Combat.Manage;

public partial class CreatureSearch : IDisposable
{
    [Inject] public IMediator Mediator { get; set; } = null!;

    [Parameter, EditorRequired] public EventCallback<ICreature> OnCreatureAdded { get; set; }

    private readonly CancellationTokenSource _cts = new();
    private CreatureSearchQueryResponse _response;
    private string _selectedCreatureName;

    protected override async Task OnInitializedAsync()
    {
        _response = await Mediator.Send(new CreatureSearchQuery(), _cts.Token);
    }

    private Task<IEnumerable<string>> Filter(string searchValue)
    {
        var allCreatures = _response.Creatures.Select(n => n.Name);

        if (string.IsNullOrEmpty(searchValue))
        {
            return Task.FromResult(allCreatures);
        }

        var lowerCaseSearchTerm = searchValue.Trim().ToLower();

        return Task.FromResult(allCreatures
            .Where(name => name.Contains(lowerCaseSearchTerm, StringComparison.InvariantCultureIgnoreCase)));
    }

    private async Task OnCreatureAddedClickedAsync()
    {
        if (_selectedCreatureName == string.Empty || _selectedCreatureName is null) return;

        var selectedCreature = _response.Creatures.First(c => c.Name == _selectedCreatureName);

        await OnCreatureAdded.InvokeAsync(selectedCreature);
    }

    public void Dispose()
    {
        _cts.Cancel();
        _cts.Dispose();
    }
}
