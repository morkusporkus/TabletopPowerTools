namespace DMPowerTools.Core.Features.Creatures.Import;

public class ImportState
{
    public ImportStatus ImportStatus { get; private set; } = ImportStatus.Upload;
    public List<Creature> InReviewCreatures = new();

    public System.Action? NotifyStateChanged;

    public void TransitionStatus()
    {
        ImportStatus = ImportStatus switch
        {
            ImportStatus.Upload => ImportStatus.Review,
            ImportStatus.Review => ImportStatus.Done,
            ImportStatus.Done => ImportStatus.Upload,
            _ => throw new NotImplementedException()
        };

        NotifyStateChanged?.Invoke();
    }
}

public enum ImportStatus
{
    Upload,
    Review,
    Done
}