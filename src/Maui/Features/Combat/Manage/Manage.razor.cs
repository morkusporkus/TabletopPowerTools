using Ardalis.SmartEnum;
using TabletopPowerTools.Core.Models;

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

    private void OnConditionRemovedClicked(InitiatedCreature initiatedCreature, InitiatedCreature.Condition condition)
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

    // TODO: Move this to Core.
    public class InitiatedCreature
    {
        public InitiatedCreature(ICreature creature)
        {
            Creature = creature;
            HitPoints = creature.RollHitPoints();
        }

        public int InitiativeRoll { get; set; }
        public ICreature Creature { get; set; }
        public int HitPoints { get; set; }

        public List<Condition> Conditions { get; set; } = new();

        // TODO:  Will need to map colors either statically or store conditions in database.
        public class Condition : SmartEnum<Condition>
        {
            public static readonly Condition Grappled = new(Icons.Material.Outlined.Link, nameof(Grappled), MudBlazor.Color.Dark, 0);

            private Condition(string icon, string name, MudBlazor.Color color, int value) : base(name, value)
            {
                Icon = icon;
                Color = color;
            }

            public string Icon { get; }
            public MudBlazor.Color Color { get; }
        }
    }

    // TODO: Move this to Core.
    // TODO: tests
    public class CombatEncounter
    {
        public System.Action OnCombatEnded { get; set; }
        public System.Action OnRoundEnded { get; set; }

        private readonly LinkedList<InitiatedCreature> _initiatedCreatures = new();
        private InitiatedCreature _currentTurnCreature;

        public int Round { get; private set; } = 1;

        public CombatEncounter()
        {
            OnRoundEnded += EndRound;
        }

        public void BeginCombat()
        {
            foreach (var creature in _initiatedCreatures)
            {
                creature.InitiativeRoll = creature.Creature.RollInitiative();
            }

            var creaturesOrderedByInitiative = _initiatedCreatures.OrderByDescending(x => x.InitiativeRoll).ToList();

            _initiatedCreatures.Clear();
            foreach (var creature in creaturesOrderedByInitiative)
            {
                _initiatedCreatures.AddLast(creature);
            }

            if (_initiatedCreatures.Any()) _currentTurnCreature = _initiatedCreatures.First();
        }

        public void Add(ICreature creature)
        {
            var initiatedCreature = new InitiatedCreature(creature);

            _initiatedCreatures.AddLast(initiatedCreature);
        }

        public void Remove(InitiatedCreature creature)
        {
            if (_currentTurnCreature == creature)
            {
                if (_initiatedCreatures.Count > 1)
                {
                    NextTurn();
                }
                else
                {
                    _currentTurnCreature = null;
                    EndCombatEncounter();
                }
            }

            _initiatedCreatures.Remove(creature);
        }

        public void PreviousTurn()
        {
            var previousActiveCreature = _initiatedCreatures.Find(_currentTurnCreature)?.Previous is null
               ? _initiatedCreatures.LastOrDefault()
               : _initiatedCreatures.Find(_currentTurnCreature).Previous.Value;

            _currentTurnCreature = previousActiveCreature;
        }

        public void NextTurn()
        {
            var isNewRound = _initiatedCreatures.Find(_currentTurnCreature)?.Next is null;

            var nextActiveCreature = isNewRound
               ? _initiatedCreatures.FirstOrDefault()
               : _initiatedCreatures.Find(_currentTurnCreature).Next.Value;

            _currentTurnCreature = nextActiveCreature;

            if (isNewRound) OnRoundEnded.Invoke();
        }

        private void EndRound()
        {
            Round++;
        }

        public void ReorderCreaturePrevious(InitiatedCreature initiatedCreature)
        {
            var node = _initiatedCreatures.Find(initiatedCreature);

            if (node.Previous is not null)
            {
                var previousNode = node.Previous;

                _initiatedCreatures.Remove(node);
                _initiatedCreatures.AddBefore(previousNode, node);
            }
        }

        public void ReorderCreatureNext(InitiatedCreature initiatedCreature)
        {
            var node = _initiatedCreatures.Find(initiatedCreature);

            if (node.Next is not null)
            {
                var nextNode = node.Next;

                _initiatedCreatures.Remove(node);
                _initiatedCreatures.AddAfter(nextNode, node);
            }
        }

        public void EndCombatEncounter()
        {
            OnCombatEnded.Invoke();
        }

        public bool IsCreaturesTurn(InitiatedCreature creature) => _currentTurnCreature == creature;

        public bool IsCombatActive() => _currentTurnCreature is not null;

        public void AddCondition(InitiatedCreature initiatedCreature, InitiatedCreature.Condition condition)
        {
            initiatedCreature.Conditions.Add(condition);
        }

        public void RemoveCondition(InitiatedCreature initiatedCreature, InitiatedCreature.Condition condition)
        {
            initiatedCreature.Conditions.Remove(condition);
        }

        public IReadOnlyCollection<InitiatedCreature> Creatures => _initiatedCreatures.ToList();
    }
}
