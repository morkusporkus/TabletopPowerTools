using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using TabletopPowerTools.Core.Features.Creatures.Import;
using TabletopPowerTools.Core.Models;

namespace TabletopPowerTools.Maui.Features.Creatures.Import;
public partial class Create
{
    [Inject] private ISnackbar Snackbar { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; }
    Creature creature = new();
    DbContext _dbContext;
    string abilityName = "";
    string abilityDescription = "";
    string actionName = "";
    string actionDescription = "";

    public Create(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    protected override void OnInitialized()
    {
        var cr = creature;
    }
    public void CreateCreature()
    {
        _dbContext.Add(creature);
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
