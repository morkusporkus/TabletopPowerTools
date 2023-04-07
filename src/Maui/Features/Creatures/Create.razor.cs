using TabletopPowerTools.Core.Models;
using TabletopPowerTools.Core.Models.ViewModels;
using TabletopPowerTools.Core.Features.Creatures;

namespace TabletopPowerTools.Maui.Features.Creatures;
public partial class Create
{
    [Inject] public IMediator Mediator { get; set; }
    [Inject] public ISnackbar Snackbar { get; set; }
    CreatureViewModel creatureViewModel = new();
    string abilityName = "";
    string abilityDescription = "";
    string actionName = "";
    string actionDescription = "";

    public async Task CreateCreature()
    {
        await Mediator.Send(new CreateCreatureCommand { Creature = creatureViewModel });
        Snackbar.Add("Creature created.", Severity.Success);

    }
    public void AddAbility()
    {
        if (creatureViewModel.Abilities.Count == 0)
        {
            creatureViewModel.Abilities = new List<AbilityViewModel>();
        }
        creatureViewModel.Abilities.Add(new() { Name = abilityName, Desc = abilityDescription });
        abilityName = "";
        abilityDescription = "";
    }
    public void AddAction()
    {
        if (creatureViewModel.Actions.Count == 0)
        {
            creatureViewModel.Actions = new List<ActionViewModel>();
        }
        creatureViewModel.Actions.Add(new() { Name = actionName, Desc = actionDescription });
        actionName = "";
        actionDescription = "";
    }
}
