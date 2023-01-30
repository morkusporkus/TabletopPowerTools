using TabletopPowerTools.Core.Features.Combat.Manage;
using TabletopPowerTools.Core.Models;
using static TabletopPowerTools.Core.Features.Combat.Manage.InitiatedCreature;

namespace TabletopPowerTools.Maui.Features.Combat.Manage;

public partial class Manage : IDisposable
{
    [Inject] public IMediator Mediator { get; set; } = null!;

    private readonly CancellationTokenSource _cts = new();
    private readonly CombatEncounter _combatEncounter = new();
    private bool _showCreatureDetails;
    private int _clickedCreatureId;

    protected override void OnInitialized()
    {
        _combatEncounter.OnCombatEnded += StateHasChanged;
        _combatEncounter.OnRoundEnded += StateHasChanged;
    }

    public void AddCreatureToEncounter(ICreature creature)
    {
        _combatEncounter.Add(creature);
    }

    private void OnRemoveFromCombatClicked(InitiatedCreature initiatedCreature)
    {
        _combatEncounter.Remove(initiatedCreature);
    }

    private void OnBeginCombatClicked()
    {
        _combatEncounter.BeginCombat();
    }

    private void OnPreviousTurnClicked()
    {
        _combatEncounter.PreviousTurn();
    }

    private void OnNextTurnClicked()
    {
        _combatEncounter.NextTurn();
    }

    private void OnReorderCreaturePreviousClicked(InitiatedCreature initiatedCreature)
    {
        _combatEncounter.ReorderCreaturePrevious(initiatedCreature);
    }

    private void OnReorderCreatureNextClicked(InitiatedCreature initiatedCreature)
    {
        _combatEncounter.ReorderCreatureNext(initiatedCreature);
    }

    private void OnConditionAddedClicked(InitiatedCreature initiatedCreature, InitiatedCreature.Condition condition)
    {
        _combatEncounter.AddCondition(initiatedCreature, condition);
    }

    private void OnConditionRemovedClicked(InitiatedCreature initiatedCreature, ActiveCondition condition)
    {
        _combatEncounter.RemoveCondition(initiatedCreature, condition);
    }

    private void OnCreatureDetailsClicked(int creatureId)
    {
        _clickedCreatureId = creatureId;
        _showCreatureDetails = true;
    }

    public void Dispose()
    {
        _cts.Cancel();
        _cts.Dispose();

        _combatEncounter.OnCombatEnded -= StateHasChanged;
    }
}
