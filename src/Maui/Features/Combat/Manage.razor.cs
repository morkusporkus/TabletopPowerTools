using Ardalis.SmartEnum;
using DMPowerTools.Core.Features.Combat;
using DMPowerTools.Core.Models;

namespace DMPowerTools.Maui.Features.Combat;

public partial class Manage : IDisposable
{
    [Inject] public IMediator Mediator { get; set; } = null!;

    private readonly CancellationTokenSource _cts = new();
    private readonly CombatEncounter _combatEncounter = new();
    private ManageCombatQueryResponse _response;
    private string _selectedCreatureName;
    private bool _creatureDetailsOpen;
    private int _clickedCreatureId;

    protected override async Task OnInitializedAsync()
    {
        _combatEncounter.OnCombatEndedCallback += StateHasChanged;

        _response = await Mediator.Send(new ManageCombatQuery(), _cts.Token);
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

    public void OnAddCreatureToEncounterClicked()
    {
        if (_selectedCreatureName == string.Empty || _selectedCreatureName is null) return;

        var selectedCreature = _response.Creatures.First(c => c.Name == _selectedCreatureName);

        _combatEncounter.Add(selectedCreature);
    }

    public void OnBeginCombatClicked()
    {
        _combatEncounter.BeginCombat();
    }

    public void OnPreviousTurnClicked()
    {
        _combatEncounter.PreviousTurn();
    }

    public void OnNextTurnClicked()
    {
        _combatEncounter.NextTurn();
    }

    public void OnRemoveFromCombatClicked(InitiatedCreature initiatedCreature)
    {
        _combatEncounter.Remove(initiatedCreature);
    }
    public void OnConditionAddedClicked(InitiatedCreature initiatedCreature, InitiatedCreature.Condition condition)
    {
        _combatEncounter.AddCondition(initiatedCreature, condition);
    }

    public void OnConditionRemovedClicked(InitiatedCreature initiatedCreature, InitiatedCreature.Condition condition)
    {
        _combatEncounter.RemoveCondition(initiatedCreature, condition);
    }

    void OpenDrawer(int creatureId)
    {
        _clickedCreatureId = creatureId;
        _creatureDetailsOpen = true;
    }

    void CloseDrawer()
    {
        _creatureDetailsOpen = false;
    }

    public void Dispose()
    {
        _cts.Cancel();
        _cts.Dispose();
        _combatEncounter.OnCombatEndedCallback -= StateHasChanged;
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
        public System.Action OnCombatEndedCallback { get; set; }

        private readonly LinkedList<InitiatedCreature> _initiatedCreatures = new();
        private InitiatedCreature? _currentTurnCreature;

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
            var nextActiveCreature = _initiatedCreatures.Find(_currentTurnCreature)?.Next is null
               ? _initiatedCreatures.FirstOrDefault()
               : _initiatedCreatures.Find(_currentTurnCreature).Next.Value;

            _currentTurnCreature = nextActiveCreature;
        }

        public void EndCombatEncounter()
        {
            OnCombatEndedCallback.Invoke();
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
