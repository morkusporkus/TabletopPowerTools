using DMPowerTools.DataModels;
using DMPowerTools.Maui.Data;

namespace DMPowerTools.Maui.DataModels
{
    public class InitiatedCreature
    {
        public InitiatedCreature(int inititiveRoll, Creature creature)
        {
            InititiveRoll = inititiveRoll;
            Creature = creature;
            HitPoints = DiceRoller.RollHitPoints(creature);
        }
        public InitiatedCreature()
        {
            Creature = new();
            Creature.Abilities = new List<Ability>();
            Creature.Actions = new List<Data.Action>();
        }
        public int InititiveRoll { get; set; }
        public Creature Creature { get; set; }
        public int HitPoints { get; set; }
    
        public string HitPointsDisplay()
        {
            return $"HP: {HitPoints} ({Creature.HitDice}d{DiceRoller.CalculateHitDieFromSize(Creature.Size)} + {DiceRoller.AdditionalHitPoints(Creature)}) ";
        }
    }
}
