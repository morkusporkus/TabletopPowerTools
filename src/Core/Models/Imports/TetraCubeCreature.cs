﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DMPowerTools.Core.Models.Imports
{
    public class TetraCubeCreature
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Size { get; set; }
        public string Type { get; set; }
        public string Tag { get; set; }
        public string Alignment { get; set; }
        public int HitDice { get; set; }
        public string ArmorName { get; set; }
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
        public int ArmorClass { get; set; }

        public ICollection<Ability> Abilities { get; set; } = Array.Empty<Ability>();
        public ICollection<Action> Actions { get; set; } = Array.Empty<Action>();

        public ICollection<Skill> Skills { get; set; } = Array.Empty<Skill>();

        public string UnderstandsBut { get; set; }
        public string ShortName { get; set; }
        public string PluralName { get; set; }
        public bool DoubleColumns { get; set; }
        public int SeparationPoint { get; set; }

    }
}
