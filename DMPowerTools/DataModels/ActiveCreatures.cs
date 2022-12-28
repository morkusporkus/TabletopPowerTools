using DMPowerTools.Data;

namespace DMPowerTools.DataModels
{
    public class ActiveCreature
    {
        public ActiveCreature(int id, int inititiveRoll, Creature creature)
        {
            Id = id;
            InititiveRoll = inititiveRoll;
            Creature = creature;
        }

        public int Id { get; set; }
        public int InititiveRoll { get; set; }
        public Creature Creature { get; set; }
    }
}
