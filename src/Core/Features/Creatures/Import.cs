using Microsoft.AspNetCore.Components.Forms;
using System.Text.Json;
using System.Text.Json.Serialization;
using Action = DMPowerTools.Core.Models.Action;

namespace DMPowerTools.Core.Features.Creatures;

// TODO: Map from/to a model not the database model.
public class AcceptCreatureCommand : IRequest<Unit>
{
    public required Creature Creature { get; set; }
}

public class AcceptCreatureCommandHandler : IRequestHandler<AcceptCreatureCommand, Unit>
{
    private readonly ApplicationDbContext _dbContext;

    public AcceptCreatureCommandHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Unit> Handle(AcceptCreatureCommand request, CancellationToken cancellationToken)
    {
        _dbContext.Add(request.Creature);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}

public class AcceptAllCreaturesCommand : IRequest<Unit>
{
    public IEnumerable<Creature> Creatures { get; set; } = Array.Empty<Creature>();
}

public class AcceptAllCreaturesCommandHandler : IRequestHandler<AcceptAllCreaturesCommand, Unit>
{
    private readonly ApplicationDbContext _dbContext;

    public AcceptAllCreaturesCommandHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Unit> Handle(AcceptAllCreaturesCommand request, CancellationToken cancellationToken)
    {
        _dbContext.AddRange(request.Creatures);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}

public class IsDuplicateCreatureQuery : IRequest<bool>
{
    public required string Name { get; set; }
}

public class IsDuplicateCreatureQueryHandler : IRequestHandler<IsDuplicateCreatureQuery, bool>
{
    private readonly ApplicationDbContext _dbContext;

    public IsDuplicateCreatureQueryHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> Handle(IsDuplicateCreatureQuery request, CancellationToken cancellationToken)
    {
        return await _dbContext.Creatures.AnyAsync(c => c.Name == request.Name, cancellationToken);
    }
}

public record ProcessMonsterFileCommand(IEnumerable<IBrowserFile> Files) : IRequest<ProcessMonsterFileCommandResponse> { }

public class ProcessMonsterFileCommandHandler : IRequestHandler<ProcessMonsterFileCommand, ProcessMonsterFileCommandResponse>
{
    private readonly IMapper _mapper;

    public ProcessMonsterFileCommandHandler(IMapper mapper)
    {
        _mapper = mapper;
    }
    public async Task<ProcessMonsterFileCommandResponse> Handle(ProcessMonsterFileCommand request, CancellationToken cancellationToken)
    {
        var creatures = new List<Creature>();
        foreach (var file in request.Files)
        {
            var tetraCubeCreature = await JsonSerializer.DeserializeAsync<TetraCubeCreature>(file.OpenReadStream(512000, default), new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }, cancellationToken);

            var creature = _mapper.Map<Creature>(tetraCubeCreature);

            creatures.Add(creature);
        }

        return new ProcessMonsterFileCommandResponse(creatures);
    }
}

public record ProcessMonsterFileCommandResponse(List<Creature> Creatures) { }

public class TetraCubeCreature
{
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

    public ICollection<Ability> Abilities { get; set; } = Array.Empty<Ability>();
    public ICollection<Action> Actions { get; set; } = Array.Empty<Action>();

    public ICollection<Skill> Skills { get; set; } = Array.Empty<Skill>();

    public string UnderstandsBut { get; set; }
    public string ShortName { get; set; }
    public string PluralName { get; set; }
    public bool DoubleColumns { get; set; }
    public int SeparationPoint { get; set; }

    public Armor Armor
    {
        get
        {
            (int baseArmorClass, ArmorType type) = ArmorName.ToLowerInvariant() switch
            {
                "none" => (10, ArmorType.None),
                "natural armor" => (10, ArmorType.None),
                "mage armor" => (13, ArmorType.Light),
                "padded" => (11, ArmorType.Light),
                "leather" => (11, ArmorType.Light),
                "studdedleather" => (12, ArmorType.Light),
                "hide" => (12, ArmorType.Medium),
                "chain shirt" => (13, ArmorType.Medium),
                "scale mail" => (14, ArmorType.Medium),
                "breastplate" => (14, ArmorType.Medium),
                "half plate" => (15, ArmorType.Medium),
                "ring mail" => (14, ArmorType.Heavy),
                "chain mail" => (16, ArmorType.Heavy),
                "splint" => (17, ArmorType.Heavy),
                "plate" => (18, ArmorType.Heavy),
                "other" => (10, ArmorType.Heavy),
                _ => (10, ArmorType.Heavy)
            };

            if (type == ArmorType.Natural) baseArmorClass += NatArmorBonus;

            var armorClass = new Armor
            {
                ArmorClassType = type,
                BaseArmorClass = baseArmorClass,
            };

            if (ShieldBonus > 0)
            {
                armorClass.Shield = new Shield { ArmorClass = ShieldBonus };
            }

            return armorClass;
        }
    }

    public AbilityScores AbilityScores
    {
        get
        {
            return new AbilityScores
            {
                Strength = StrPoints,
                Dexterity = DexPoints,
                Constitution = ConPoints,
                Intelligence = IntPoints,
                Wisdom = WisPoints,
                Charisma = ChaPoints
            };
        }
    }

    public class TetraCubeCreatureProfile : Profile
    {
        public TetraCubeCreatureProfile()
        {
            CreateMap<TetraCubeCreature, Creature>()
                .ForMember(d => d.Id, mo => mo.Ignore());
        }
    }
}