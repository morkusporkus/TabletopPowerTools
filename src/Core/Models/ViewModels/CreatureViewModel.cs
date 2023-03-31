﻿namespace TabletopPowerTools.Core.Models.ViewModels;

public class CreatureViewModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Size { get; set; }
    public int HitDice { get; set; }
    public string Alignment { get; set; }
    public AbilityScores AbilityScores { get; set; } = new();
    public Armor Armor { get; set; } = new() { ArmorClassType = new(), BaseArmorClass = new() };
    public int Speed { get; set; }
    public string Cr { get; set; }

    public List<Ability> Abilities { get; set; } = new();
    public List<Action> Actions { get; set; } = new();
    public List<Skill> Skills { get; set; } = new();
}
