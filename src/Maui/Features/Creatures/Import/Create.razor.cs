using TabletopPowerTools.Core.Features.Creatures.Import;
using TabletopPowerTools.Core.Models;

namespace TabletopPowerTools.Maui.Features.Creatures.Import;
public partial class Create
{
    [Inject] public IMediator Mediator { get; set; }
    Creature creature = new();
    string abilityName = "";
    string abilityDescription = "";
    string actionName = "";
    string actionDescription = "";

    public async void CreateCreature()
    {
        await Mediator.Send(new AcceptCreatureCommand { Creature = creature });
    }
    public void AddAbility()
    {
        if (creature.Abilities.Count == 0)
        {
            creature.Abilities = new List<Ability>();
        }
        creature.Abilities.Add(new() { Name = abilityName, Desc = abilityDescription });
        abilityName = "";
        abilityDescription = "";
    }
    public void AddAction()
    {
        if (creature.Actions.Count == 0)
        {
            creature.Actions = new List<Core.Models.Action>();
        }
        creature.Actions.Add(new() { Name = actionName, Desc = actionDescription });
        actionName = "";
        actionDescription = "";
    }
}
