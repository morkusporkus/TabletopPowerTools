using DMPowerTools.Data;

namespace DMPowerTools.DataModels
{
    public class DiceRoller
    {
        private Random _random = new Random();
        public int RollHitPoints(Creature creature)
        {
            var totalRolls = 0;
            for (int j = 0; j < creature.HitDice; j++)
            {
                totalRolls = totalRolls + _random.Next(1, SizeToDiceConverter(creature.Size));
            }
            return totalRolls + (CalculateAbilityScoreModifer(creature.ConPoints) * creature.HitDice);
        }
        public int CalculateAbilityScoreModifer(int abilityScore)
        {
            return (abilityScore - 10) / 2;
        }
        public int RollForInitiative(int dex)
        {
            var roll = _random.Next(1, 20);
            return roll + CalculateAbilityScoreModifer(dex);
        }
        public int AdditionalHitPoints(Creature creature)
        {
            return CalculateAbilityScoreModifer(creature.ConPoints) * creature.HitDice;
        }
        public int SizeToDiceConverter(string size)
        {
            switch (size.ToLower())
            {
                case "tiny":
                    return 4;
                case "small":
                    return 6;
                case "medium":
                    return 8;
                case "large":
                    return 10;
                case "huge":
                    return 12;
                case "gargantuan":
                    return 20;
                default:
                    return 8;
            }

        }

    }
}
