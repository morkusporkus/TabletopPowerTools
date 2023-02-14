namespace TabletopPowerTools.Maui.Shared;

public class RpgIcons
{
    private const string _baseClass = "game-icon game-icon-";

    public class DamageType
    {
        public const string Acid = _baseClass + "acid";
        public const string Bludgeoning = _baseClass + "thor-hammer";
        public const string Cold = _baseClass + "snowflake-1";
        public const string Fire = _baseClass + "fire";
        public const string Force = _baseClass + "ringed-beam";
        public const string Lightning = _baseClass + "lightning-storm";
        public const string Necrotic = _baseClass + "broken-skull";
        public const string Piercing = _baseClass + "spear-hook";
        public const string Psychic = _baseClass + "psychic-waves";
        public const string Radiant = _baseClass + "sun-radiations";
        public const string Slashing = _baseClass + "saber-slash";
        public const string Thunder = _baseClass + "earth-spit";
    }

    public class Creature
    {
        public const string ArmorClass = _baseClass + "chest-armor";
        public const string Movement = _baseClass + "walking-boot";
        public const string HitPoints = _baseClass + "hearts";

        public class Attribute
        {
            public const string Strength = _baseClass + "lion";
            public const string Dexterity = _baseClass + "feline";
            public const string Constitution = _baseClass + "bear-head";
            public const string Wisdom = _baseClass + "owl";
            public const string Intelligence = _baseClass + "fox-head";
            public const string Charisma = _baseClass + "eagle-head";
        }

        public class Skill
        {
            public const string Acrobatics = _baseClass + "contortionist";
            public const string AnimalHandling = _baseClass + "cavalry";
            public const string Arcana = _baseClass + "spell-book";
            public const string Athletics = _baseClass + "muscle-up";
            public const string Deception = _baseClass + "convince";
            public const string History = _baseClass + "backward-time";
            public const string Insight = _baseClass + "inner-self";
            public const string Intimidation = _baseClass + "one-eyed";
            public const string Investigation = _baseClass + "magnifying-glass";
            public const string Medicine = _baseClass + "apothecary";
            public const string Nature = _baseClass + "forest";
            public const string Perception = _baseClass + "semi-closed-eye";
            public const string Performance = _baseClass + "sing";
            public const string Persuasion = _baseClass + "public-speaker";
            public const string Religion = _baseClass + "ankh";
            public const string SleightOfHand = _baseClass + "snatch";
            public const string Stealth = _baseClass + "cloak-dagger";
            public const string Survival = _baseClass + "deer-track";
        }

        public class Condition
        {
            public const string Grappled = _baseClass + "grapple";
            public const string Haste = _baseClass + "wingfoot";
        }
    }

    public class Miscellaneous
    {
        public const string D20 = _baseClass + "dice-twenty-faces-twenty";
    }
}
