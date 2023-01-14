using DMPowerTools.Core.Features.Creatures;

namespace DMPowerTools.Maui.Features.Creatures;
public partial class List : IDisposable
{
    [Inject] public IMediator Mediator { get; set; }

    private readonly CancellationTokenSource _cts = new();
    private ListQueryResponse _response;

    protected override async Task OnInitializedAsync()
    {
        _response = await Mediator.Send(new ListQuery(), _cts.Token);
    }

    private async Task DeleteCreatureAsync(int id)
    {
        await Mediator.Send(new DeleteCommand { Id = id }, _cts.Token);

        var deletedCreature = _response.Creatures.First(c => c.Id == id);

        _response.Creatures.Remove(deletedCreature);
    }

    public void Dispose()
    {
        _cts.Cancel();
        _cts.Dispose();
    }
}
