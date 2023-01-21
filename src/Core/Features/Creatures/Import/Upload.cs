using Microsoft.AspNetCore.Components.Forms;
using System.Text.Json;
using System.Text.Json.Serialization;
using Action = TabletopPowerTools.Core.Models.Action;

namespace TabletopPowerTools.Core.Features.Creatures.Import;

public record UploadMonsterFileCommand(IEnumerable<IBrowserFile> Files, UploadSource UploadSource) : IRequest<UploadMonsterFileCommandResponse> { }

public class UploadMonsterFileCommandHandler : IRequestHandler<UploadMonsterFileCommand, UploadMonsterFileCommandResponse>
{
    private readonly IUploadStrategyFactory _uploadStrategyFactory;

    public UploadMonsterFileCommandHandler(IUploadStrategyFactory uploadStrategyFactory)
    {
        _uploadStrategyFactory = uploadStrategyFactory;
    }

    public async Task<UploadMonsterFileCommandResponse> Handle(UploadMonsterFileCommand request, CancellationToken cancellationToken)
    {
        var uploadStrategy = _uploadStrategyFactory.GetUploadStrategy(request.UploadSource)!;

        var creatures = new List<Creature>();
        foreach (var file in request.Files)
        {
            creatures.AddRange(await uploadStrategy.DeserializeAsync(file, cancellationToken));
        }

        return new UploadMonsterFileCommandResponse(creatures);
    }
}

public enum UploadSource
{
    TetraCube,
    TableTopPowerTools
}

public interface IUploadStrategyFactory
{
    IUploadStrategy GetUploadStrategy(UploadSource uploadSource);
}

public class UploadStrategyFactory : IUploadStrategyFactory
{
    private readonly IEnumerable<IUploadStrategy> _uploadStrategies;

    public UploadStrategyFactory(IEnumerable<IUploadStrategy> uploadStrategies)
    {
        _uploadStrategies = uploadStrategies;
    }

    public IUploadStrategy GetUploadStrategy(UploadSource uploadSource)
    {
        return uploadSource switch
        {
            UploadSource.TetraCube => _uploadStrategies.First(us => us is TetraCubeUploadStrategy),
            UploadSource.TableTopPowerTools => _uploadStrategies.First(us => us is TableTopPowerToolsUploadStrategy),
            _ => throw new ArgumentOutOfRangeException(nameof(uploadSource), uploadSource, "No upload strategy for given value.")
        };
    }
}

public record UploadMonsterFileCommandResponse(List<Creature> Creatures) { }

public interface IUploadStrategy
{
    public ValueTask<List<Creature>> DeserializeAsync(IBrowserFile file, CancellationToken cancellationToken);
}

public class TableTopPowerToolsUploadStrategy : IUploadStrategy
{
    public async ValueTask<List<Creature>> DeserializeAsync(IBrowserFile file, CancellationToken cancellationToken)
    {
        var ttptCreatures = await JsonSerializer.DeserializeAsync<List<Creature>>(file.OpenReadStream(cancellationToken: cancellationToken), new JsonSerializerOptions(JsonSerializerDefaults.Web), cancellationToken);

        return ttptCreatures ?? new();
    }
}
public class TetraCubeUploadStrategy : IUploadStrategy
{
    private readonly IMapper _mapper;

    public TetraCubeUploadStrategy(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async ValueTask<List<Creature>> DeserializeAsync(IBrowserFile file, CancellationToken cancellationToken)
    {
        var tetraCubeCreature = await JsonSerializer.DeserializeAsync<TetraCubeCreature>(file.OpenReadStream(cancellationToken: cancellationToken), new JsonSerializerOptions(JsonSerializerDefaults.Web), cancellationToken);

        var creature = _mapper.Map<Creature>(tetraCubeCreature);

        return new List<Creature> { creature };
    }

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
}