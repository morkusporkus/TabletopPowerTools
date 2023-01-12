using System.Drawing;
using System.Reflection;
using System.Text.Json.Serialization;

namespace DMPowerTools.Core.Models;

// TODO: shim? don't want to keep this if possible.
public interface ICreature
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int HitDice { get; set; }
    public string Size { get; set; }
    public int DexPoints { get; set; }
    public int ConPoints { get; set; }
    public string Cr { get; set; }
    public int AC { get; set; }
}

public class Creature : ICreature
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Size { get; set; }
    public int HitDice { get; set; }
    public string Type { get; set; }
    public string Tag { get; set; }
    public string Alignment { get; set; }
    public int AC { get; set; }
    public int ShieldBonus { get; set; }
    public int Speed { get; set; }
    public int BurrowSpeed { get; set; }
    public int ClimbSpeed { get; set; }
    public int FlySpeed { get; set; }
    public bool Hover { get; set; }
    public int SwimSpeed { get; set; }
    public bool CustomHP { get; set; }
    public bool CustomSpeed { get; set; }
    public string HpText { get; set; }
    public string SpeedDesc { get; set; }
    public int StrPoints { get; set; }
    public int DexPoints { get; set; }
    public int ConPoints { get; set; }
    public int IntPoints { get; set; }
    public int WisPoints { get; set; }
    public int ChaPoints { get; set; }
    public int Blindsight { get; set; }
    public bool Blind { get; set; }
    public int Darkvision { get; set; }
    public int Tremorsense { get; set; }
    public int Truesight { get; set; }
    public int Telepathy { get; set; }
    [JsonPropertyName("cr")]
    public string Cr { get; set; }
    public string CustomCr { get; set; }
    public int CustomProf { get; set; }
    public bool IsLegendary { get; set; }
    public string LegendariesDescription { get; set; }
    public bool IsLair { get; set; }
    public string LairDescription { get; set; }
    public string LairDescriptionEnd { get; set; }
    public bool IsMythic { get; set; }
    public string MythicDescription { get; set; }
    public bool IsRegional { get; set; }
    public string RegionalDescription { get; set; }
    public string RegionalDescriptionEnd { get; set; }
    public ICollection<Ability> Abilities { get; set; } = Array.Empty<Ability>();
    public ICollection<Action> Actions { get; set; } = Array.Empty<Action>();
    public ICollection<Skill> Skills { get; set; } = Array.Empty<Skill>();

    public string UnderstandsBut { get; set; }
    public string ShortName { get; set; }
    public string PluralName { get; set; }
    public bool DoubleColumns { get; set; }
    public int SeparationPoint { get; set; }

    public int CalculateACFromTetraCube(string armorType, int dex, int naturalArmorBonus)
    {
        var modifier = CalculateAbilityScoreModifier(dex);
        var modifierWithCap = modifier;
        if (modifier > 2)
        {
            modifierWithCap = 2;
        }

        return armorType.ToLowerInvariant() switch
        {
            "none" => 10 + modifier + naturalArmorBonus,
            "natural armor" => 10 + modifier + naturalArmorBonus,
            "mage armor" => 13 + modifier,
            "padded" => 11 + modifier,
            "leather" => 11 + modifier,
            "studdedleather" => 12 + modifier,
            "hide" => 12 + modifierWithCap,
            "chain shirt" => 13 + modifierWithCap,
            "scale mail" => 14 + modifierWithCap,
            "breastplate" => 14 + modifierWithCap,
            "half plate" => 15 + modifierWithCap,
            "ring mail" => 14,
            "chain mail" => 16,
            "splint" => 17,
            "plate" => 18,
            "other" => 10,
            _ => 10,
        };
    }
    public static int CalculateAbilityScoreModifier(int abilityScore)
    {
        return (abilityScore - 10) / 2;
    }

}

public static class CreatureExtensions
{
    private static readonly Random _random = new();

    public static int RollHitPoints(this ICreature creature)
    {
        var totalHitPoints = 0;
        for (int j = 0; j < creature.HitDice; j++)
        {
            totalHitPoints += _random.Next(1, CalculateHitDieFromSize(creature.Size));
        }

        return totalHitPoints + Creature.CalculateAbilityScoreModifier(creature.ConPoints) * creature.HitDice;
    }

    public static int RollInitiative(this ICreature creature)
    {
        var roll = _random.Next(1, 20);

        return roll + Creature.CalculateAbilityScoreModifier(creature.DexPoints);
    }

    private static int CalculateHitDieFromSize(string size)
    {
        return size.ToLowerInvariant() switch
        {
            "tiny" => 4,
            "small" => 6,
            "medium" => 8,
            "large" => 10,
            "huge" => 12,
            "gargantuan" => 20,
            _ => 8,
        };
    }


}
