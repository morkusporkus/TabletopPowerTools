using DMPowerTools.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMPowerTools.DataModels
{
    public class DiceRoller
    {
        private Random _random= new Random();
        public int RollHitPoints(Creature creature)
        {
            var totalRolls = 0;
            for (int j = 0; j < creature.HitDice; j++)
            {
                totalRolls = totalRolls + _random.Next(1,SizeToDiceConverter(creature.Size));
            }
            return totalRolls + (CalculateAbilityScoreModifer(creature.ConPoints) * creature.HitDice);
        }
        public int CalculateAbilityScoreModifer(int abilityScore)
        {
            return (abilityScore - 10) / 2;
        }
        public int RollForInitiative(int dex)
        {
            var roll =_random.Next(1, 20);
            return roll + CalculateAbilityScoreModifer(dex);
        }
        public int AdditionalHitPoints(Creature creature)
        {
           return CalculateAbilityScoreModifer(creature.ConPoints) * creature.HitDice;
        }
        public int SizeToDiceConverter(string size)
        {
            switch (size)
            {
                case "Tiny":
                    return 4;
                case "Small":
                    return 6;
                case "Medium":
                    return 8;
                case "Large":
                    return 10;
                case "Huge":
                    return 12;
                case "Gargantuan":
                    return 20;
                default:
                    return 8;
            }

        }

    }
}
