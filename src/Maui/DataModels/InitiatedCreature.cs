using DMPowerTools.Data;

namespace DMPowerTools.DataModels
{
    public class InitiatedCreature
    {
        public InitiatedCreature(int inititiveRoll, Creature creature)
        {
            InititiveRoll = inititiveRoll;
            Creature = creature;
        }

        public int InititiveRoll { get; set; }
        public Creature Creature { get; set; }
    }
}
