namespace TabletopPowerTools.Core.Models;


// TODO: shim? don't want to keep this if possible.
public interface ICreature
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int HitDice { get; set; }
    public string Size { get; set; }
    public AbilityScores AbilityScores { get; set; }
    public string Cr { get; set; }
    public int ArmorClass { get; }
}

public class Creature : ICreature
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Size { get; set; }
    public int HitDice { get; set; }
    public string Alignment { get; set; }
    public AbilityScores AbilityScores { get; set; } = new();
    public Armor Armor { get; set; }
    public int Speed { get; set; }
    public string Cr { get; set; }

    public int ArmorClass => Armor.Calculate(AbilityScores.Dexterity);

    public ICollection<Ability> Abilities { get; set; } = Array.Empty<Ability>();
    public ICollection<Action> Actions { get; set; } = Array.Empty<Action>();
    public ICollection<Skill> Skills { get; set; } = Array.Empty<Skill>();
}

[Owned]
public class AbilityScores
{
    public int Strength { get; set; }
    public int Dexterity { get; set; }
    public int Constitution { get; set; }
    public int Intelligence { get; set; }
    public int Wisdom { get; set; }
    public int Charisma { get; set; }

    public static int CalculateModifier(int abilityScore)
    {
        return (abilityScore - 10) / 2;
    }
}

[Owned]
public class Armor
{
    public required int BaseArmorClass { get; set; }
    public required ArmorType ArmorClassType { get; set; }
    public Shield? Shield { get; set; }

    public int Calculate(int dexterityModifier)
    {
        var armorClassTypeBonus = CalculateArmorTypeBonus(dexterityModifier);

        return BaseArmorClass + armorClassTypeBonus + (Shield?.ArmorClass ?? 0);
    }

    private int CalculateArmorTypeBonus(int dexterityModifier)
    {
        var modifierCeiling = ArmorClassType switch
        {
            ArmorType.None or ArmorType.Light or ArmorType.Natural => int.MaxValue,
            ArmorType.Medium => 2,
            ArmorType.Heavy => 0,
            _ => throw new NotImplementedException("Missing armor class type")
        };

        return Math.Min(modifierCeiling, dexterityModifier);
    }
}

public enum ArmorType
{
    None,
    Natural,
    Light,
    Medium,
    Heavy
}

[Owned]
public class Shield
{
    public required int ArmorClass { get; set; }
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

        return totalHitPoints + AbilityScores.CalculateModifier(creature.AbilityScores.Constitution) * creature.HitDice;
    }

    public static int RollInitiative(this ICreature creature)
    {
        var roll = _random.Next(1, 20);

        return roll + AbilityScores.CalculateModifier(creature.AbilityScores.Dexterity);
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
