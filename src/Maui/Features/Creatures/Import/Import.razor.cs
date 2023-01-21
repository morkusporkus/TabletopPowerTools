using TabletopPowerTools.Core.Features.Creatures.Import;

namespace TabletopPowerTools.Maui.Features.Creatures.Import;
public partial class Import : IDisposable
{
    [Inject] private ISnackbar Snackbar { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; }

    private readonly ImportState _importState = new();

    protected override void OnInitialized()
    {
        _importState.NotifyStateChanged += OnImportStatusChanged;
    }

    private void OnImportStatusChanged()
    {
        if (_importState.ImportStatus == ImportStatus.Done)
        {
            Snackbar.Add("Finished importing all creatures.", Severity.Success);

            NavigationManager.NavigateTo("/");
        }

        StateHasChanged();
    }

    public void Dispose()
    {
        _importState.NotifyStateChanged -= OnImportStatusChanged;
    }
}
