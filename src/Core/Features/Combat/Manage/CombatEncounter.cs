using Ardalis.SmartEnum;
using TabletopPowerTools.Maui.Shared;
using static TabletopPowerTools.Core.Features.Combat.Manage.InitiatedCreature;

namespace TabletopPowerTools.Core.Features.Combat.Manage;


public class CombatEncounter
{
    public System.Action? OnCombatEnded { get; set; }
    public System.Action? OnRoundEnded { get; set; }

    private readonly LinkedList<InitiatedCreature> _initiatedCreatures = new();
    private InitiatedCreature? _currentTurnCreature;

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

        if (isNewRound) OnRoundEnded?.Invoke();
    }

    private void EndRound()
    {
        Round++;

        foreach (var creature in _initiatedCreatures)
        {
            var eligibleConditionsToBeRemoved = creature.ActiveConditions.Where(c => c.RoundsRemaining > 0);

            foreach (var condition in eligibleConditionsToBeRemoved)
            {
                condition.RoundsRemaining--;
            }

            creature.ActiveConditions.RemoveAll(c => c.RoundsRemaining == 0);
        }
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
        Round = 1;

        OnCombatEnded?.Invoke();
    }

    public bool IsCreaturesTurn(InitiatedCreature creature) => _currentTurnCreature == creature;

    public bool IsCombatActive() => _currentTurnCreature is not null;

    public void AddCondition(InitiatedCreature initiatedCreature, Condition condition)
    {
        initiatedCreature.ActiveConditions.Add(new ActiveCondition(condition));
    }

    public void RemoveCondition(InitiatedCreature initiatedCreature, ActiveCondition condition)
    {
        initiatedCreature.ActiveConditions.Remove(condition);
    }

    public IReadOnlyCollection<InitiatedCreature> Creatures => _initiatedCreatures.ToList();
}

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

    public List<ActiveCondition> ActiveConditions { get; set; } = new();
    public IEnumerable<Condition> InactiveConditions => Condition.List.Where(c => !ActiveConditions.Any(ac => ac.Condition.Value == c.Value));

    public class ActiveCondition
    {
        public ActiveCondition(Condition condition)
        {
            Condition = condition;
            RoundsRemaining = condition.RoundDuration;
        }

        public Condition Condition { get; set; }
        public int? RoundsRemaining { get; set; }
    }

    public class Condition : SmartEnum<Condition>
    {
        private static readonly int? _indefiniteRoundDuration = null;

        public static readonly Condition Grappled = new(RpgIcons.Creature.Condition.Grappled, nameof(Grappled), MudBlazor.Color.Dark, _indefiniteRoundDuration, 0);
        public static readonly Condition Haste = new(RpgIcons.Creature.Condition.Haste, nameof(Haste), MudBlazor.Color.Success, 10, 1);

        private Condition(string icon, string name, MudBlazor.Color color, int? roundDuration, int value) : base(name, value)
        {
            Icon = icon;
            Color = color;
            RoundDuration = roundDuration;
        }

        public string Icon { get; }
        public MudBlazor.Color Color { get; }
        public int? RoundDuration { get; set; }
    }
}