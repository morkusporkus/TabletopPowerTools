namespace DMPowerTools.Maui.Features.Creatures.Import;
public partial class Import : IDisposable
{
    [Inject] private ISnackbar Snackbar { get; set; }

    private readonly ImportState _importState = new();

    protected override void OnInitialized()
    {
        _importState.NotifyStateChanged += OnImportStatusChanged;
    }

    private void OnImportStatusChanged()
    {
        if (_importState.ImportStatus == ImportStatus.Done)
        {
            Snackbar.Add("Done! Upload more creatures to begin again.", Severity.Success);
            _importState.TransitionStatus();
        }

        StateHasChanged();
    }

    public void Dispose()
    {
        _importState.NotifyStateChanged -= OnImportStatusChanged;
    }
}

public class ImportState
{
    public ImportStatus ImportStatus { get; private set; } = ImportStatus.Upload;
    public List<Creature> InReviewCreatures = new();

    public System.Action NotifyStateChanged;

    public void TransitionStatus()
    {
        ImportStatus = ImportStatus switch
        {
            ImportStatus.Upload => ImportStatus.Review,
            ImportStatus.Review => ImportStatus.Done,
            ImportStatus.Done => ImportStatus.Upload,
            _ => throw new NotImplementedException()
        };

        NotifyStateChanged.Invoke();
    }
}

public enum ImportStatus
{
    Upload,
    Review,
    Done
}
