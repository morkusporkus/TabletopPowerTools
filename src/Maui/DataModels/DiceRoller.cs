using DMPowerTools.Maui.Data;

namespace DMPowerTools.DataModels
{
    public class DiceRoller
    {
        private static readonly Random _random = new();

        public static int RollHitPoints(Creature creature)
        {
            var totalHitPoints = 0;
            for (int j = 0; j < creature.HitDice; j++)
            {
                totalHitPoints += _random.Next(1, CalculateHitDieFromSize(creature.Size));
            }

            return totalHitPoints + (CalculateAbilityScoreModifier(creature.ConPoints) * creature.HitDice);
        }

        public static int CalculateAbilityScoreModifier(int abilityScore)
        {
            return (abilityScore - 10) / 2;
        }

        public static int RollForInitiative(int dex)
        {
            var roll = _random.Next(1, 20);
            return roll + CalculateAbilityScoreModifier(dex);
        }

        public static int AdditionalHitPoints(Creature creature)
        {
            return CalculateAbilityScoreModifier(creature.ConPoints) * creature.HitDice;
        }

        public static int CalculateHitDieFromSize(string size)
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
}
