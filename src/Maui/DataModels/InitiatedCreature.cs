using DMPowerTools.Data;

namespace DMPowerTools.DataModels
{
    public class InitiatedCreature
    {
        DiceRoller diceRoller = new();
        public InitiatedCreature(int inititiveRoll, Creature creature)
        {
            InititiveRoll = inititiveRoll;
            Creature = creature;
            HitPoints= diceRoller.RollHitPoints(creature);
            HitPointsDisplay = $"HP: {HitPoints} ({creature.HitDice}d{diceRoller.SizeToDiceConverter(creature.Size)} + {diceRoller.AdditionalHitPoints(creature)}) ";
        }

        public int InititiveRoll { get; set; }
        public Creature Creature { get; set; }
        public int HitPoints { get; set; }
        public string HitPointsDisplay { get; set; }
    }
}
