using DMPowerTools.Core.Features.Combat;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using System.Threading;

namespace DMPowerTools.Core.Models.Imports
{
    public class TetraCubeCreature
    {
        private readonly IMapper _mapper;

        public TetraCubeCreature(IMapper mapper)
        {
            _mapper = mapper;
        }

        public string Name { get; set; }
        public string Size { get; set; }
        public string Type { get; set; }
        public string Tag { get; set; }
        public string Alignment { get; set; }
        public int HitDice { get; set; }
        public string ArmorName { get; set; }
        public int ArmorClass { get; set; }
        public int ShieldBonus { get; set; }
        public int NatArmorBonus { get; set; }
        public string OtherArmorDesc { get; set; }
        public int Speed { get; set; }
        public int BurrowSpeed { get; set; }
        public int ClimbSpeed { get; set; }
        public int FlySpeed { get; set; }
        public bool Hover { get; set; }
        public int SwimSpeed { get; set; }
        public bool CustomHP { get; set; }
        public bool CustomSpeed { get; set; }
        public string HpText { get; set; }
        public string SpeedDesc { get; set; }
        public int StrPoints { get; set; }
        public int DexPoints { get; set; }
        public int DexModifier
        {
            get { return (DexPoints - 10) / 2; }
            set { DexPoints = value / 2; }
        }
        public int ConPoints { get; set; }
        public int IntPoints { get; set; }
        public int WisPoints { get; set; }
        public int ChaPoints { get; set; }
        public int Blindsight { get; set; }
        public bool Blind { get; set; }
        public int Darkvision { get; set; }
        public int Tremorsense { get; set; }
        public int Truesight { get; set; }
        public int Telepathy { get; set; }
        [JsonPropertyName("cr")]
        public string Cr { get; set; }
        public string CustomCr { get; set; }
        public int CustomProf { get; set; }
        public bool IsLegendary { get; set; }
        public string LegendariesDescription { get; set; }
        public bool IsLair { get; set; }
        public string LairDescription { get; set; }
        public string LairDescriptionEnd { get; set; }
        public bool IsMythic { get; set; }
        public string MythicDescription { get; set; }
        public bool IsRegional { get; set; }
        public string RegionalDescription { get; set; }
        public string RegionalDescriptionEnd { get; set; }

        public ICollection<Ability> Abilities { get; set; } = Array.Empty<Ability>();
        public ICollection<Action> Actions { get; set; } = Array.Empty<Action>();

        public ICollection<Skill> Skills { get; set; } = Array.Empty<Skill>();

        public string UnderstandsBut { get; set; }
        public string ShortName { get; set; }
        public string PluralName { get; set; }
        public bool DoubleColumns { get; set; }
        public int SeparationPoint { get; set; }

        public Creature ConvertTetraCubeCreatureToCreature()
        {
           Creature creature =  _mapper.Map(this, new Creature());

           return creature;
        }
    }
    public static class TetraCubeCreatureExtensions
    {

        public static int CalculateArmorClass(this TetraCubeCreature creature)
        {
            var modifierWithCap = creature.DexModifier;
            if (modifierWithCap > 2)
            {
                modifierWithCap = 2;
            }

            return creature.ArmorName.ToLowerInvariant() switch
            {
                "none" => 10 + creature.DexModifier + creature.NatArmorBonus + creature.ShieldBonus,
                "natural armor" => 10 + creature.DexModifier + creature.NatArmorBonus + creature.ShieldBonus,
                "mage armor" => 13 + creature.DexModifier + creature.ShieldBonus,
                "padded" => 11 + creature.DexModifier + creature.ShieldBonus,
                "leather" => 11 + creature.DexModifier + creature.ShieldBonus,
                "studdedleather" => 12 + creature.DexModifier + creature.ShieldBonus,
                "hide" => 12 + modifierWithCap + creature.ShieldBonus,
                "chain shirt" => 13 + modifierWithCap + creature.ShieldBonus,
                "scale mail" => 14 + modifierWithCap + creature.ShieldBonus,
                "breastplate" => 14 + modifierWithCap + creature.ShieldBonus,
                "half plate" => 15 + modifierWithCap + creature.ShieldBonus,
                "ring mail" => 14 + creature.ShieldBonus,
                "chain mail" => 16 + creature.ShieldBonus,
                "splint" => 17 + creature.ShieldBonus,
                "plate" => 18 + creature.ShieldBonus,
                "other" => 10 + creature.ShieldBonus,
                _ => 10 + creature.ShieldBonus,
            };
        }
    }

    public class TetraCubeCreatureProfile : Profile
    {
        public TetraCubeCreatureProfile()
        {
            CreateMap<TetraCubeCreature, Creature>()
                .ForMember(c=> c.ArmorClass,tcc=>tcc.Ignore())
                .ForMember(c => c.Id, tcc => tcc.Ignore());
        }
    }

}
