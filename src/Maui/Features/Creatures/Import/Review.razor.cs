namespace DMPowerTools.Maui.Features.Creatures.Import;
public partial class Review
{
    [Inject] public ISnackbar Snackbar { get; set; }
    [Inject] public ApplicationDbContext DbContext { get; set; }
    [CascadingParameter] public ImportState ImportState { get; set; }

    private Creature _inReviewCreature;

    protected override void OnInitialized()
    {
        AssignNextCreature();
    }

    private async Task AcceptAsync()
    {
        DbContext.Creatures.Add(_inReviewCreature);
        await DbContext.SaveChangesAsync();

        AssignNextCreature();
    }

    private void Deny() => AssignNextCreature();

    private async Task AcceptAllAsync()
    {
        DbContext.Creatures.AddRange(ImportState.InReviewCreatures);
        await DbContext.SaveChangesAsync();

        ImportState.TransitionStatus();
    }

    private void AssignNextCreature()
    {
        if (_inReviewCreature != null)
        {
            ImportState.InReviewCreatures.Remove(_inReviewCreature);
        }

        _inReviewCreature = ImportState.InReviewCreatures.FirstOrDefault();

        if (_inReviewCreature == null)
        {
            ImportState.TransitionStatus();
        }
        else if (DbContext.Creatures.Where(c => c.Name == _inReviewCreature.Name).FirstOrDefault() != null)
        {
            Snackbar.Add($"Duplicate {_inReviewCreature.Name} already exists", Severity.Warning);
        }
    }
}
