using System;
using SQLite;
namespace DMPowerTools.Database;
public class MonsterItem
{
    [PrimaryKey, AutoIncrement]
    public int ID { get; set; }
    public string name { get; set; }
    public string size { get; set; }
    public string type { get; set; }
    public string tag { get; set; }
    public string alignment { get; set; }
    public int hitDice { get; set; }
    public string armorName { get; set; }
    public int shieldBonus { get; set; }
    public int natArmorBonus { get; set; }
    public string otherArmorDesc { get; set; }
    public int speed { get; set; }
    public int burrowSpeed { get; set; }
    public int climbSpeed { get; set; }
    public int flySpeed { get; set; }
    public bool hover { get; set; }
    public int swimSpeed { get; set; }
    public bool customHP { get; set; }
    public bool customSpeed { get; set; }
    public string hpText { get; set; }
    public string speedDesc { get; set; }
    public int strPoints { get; set; }
    public int dexPoints { get; set; }
    public int conPoints { get; set; }
    public int intPoints { get; set; }
    public int wisPoints { get; set; }
    public int chaPoints { get; set; }
    public int blindsight { get; set; }
    public bool blind { get; set; }
    public int darkvision { get; set; }
    public int tremorsense { get; set; }
    public int truesight { get; set; }
    public int telepathy { get; set; }
    public string cr { get; set; }
    public string customCr { get; set; }
    public int customProf { get; set; }
    public bool isLegendary { get; set; }
    public string legendariesDescription { get; set; }
    public bool isLair { get; set; }
    public string lairDescription { get; set; }
    public string lairDescriptionEnd { get; set; }
    public bool isMythic { get; set; }
    public string mythicDescription { get; set; }
    public bool isRegional { get; set; }
    public string regionalDescription { get; set; }
    public string regionalDescriptionEnd { get; set; }
    public object[] properties { get; set; }
    public AbilityItem[] abilities { get; set; }
    public ActionItem[] actions { get; set; }
    public object[] bonusActions { get; set; }
    public object[] reactions { get; set; }
    public object[] legendaries { get; set; }
    public object[] mythics { get; set; }
    public object[] lairs { get; set; }
    public object[] regionals { get; set; }
    public object[] sthrows { get; set; }
    public SkillItem[] skills { get; set; }
    public object[] damagetypes { get; set; }
    public object[] specialdamage { get; set; }
    public object[] conditions { get; set; }
    public object[] languages { get; set; }
    public string understandsBut { get; set; }
    public string shortName { get; set; }
    public string pluralName { get; set; }
    public bool doubleColumns { get; set; }
    public int separationPoint { get; set; }
    public object[] damage { get; set; }

}

public class AbilityItem
{
    [PrimaryKey, AutoIncrement]
    public int ID { get; set; }
    public string name { get; set; }
    public string desc { get; set; }
}

public class ActionItem
{
    [PrimaryKey, AutoIncrement]
    public int ID { get; set; }
    public string name { get; set; }
    public string desc { get; set; }
}

public class SkillItem
{
    [PrimaryKey, AutoIncrement]
    public int ID { get; set; }
    public string name { get; set; }
    public string stat { get; set; }
}