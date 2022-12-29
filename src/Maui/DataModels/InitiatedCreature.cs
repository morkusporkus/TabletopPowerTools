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
            HitPoints = diceRoller.RollHitPoints(creature);
        }

        public int InititiveRoll { get; set; }
        public Creature Creature { get; set; }
        public int HitPoints { get; set; }
        public string HitPointsDisplay()
        {
            return $"HP: {HitPoints} ({Creature.HitDice}d{diceRoller.SizeToDiceConverter(Creature.Size)} + {diceRoller.AdditionalHitPoints(Creature)}) ";
        }
    }
}
