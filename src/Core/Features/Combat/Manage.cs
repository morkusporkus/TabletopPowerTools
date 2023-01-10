namespace DMPowerTools.Core.Features.Combat;

public class ManageCombatQuery : IRequest<ManageCombatQueryResponse> { }

public class ManageCombatQueryHandler : IRequestHandler<ManageCombatQuery, ManageCombatQueryResponse>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    public ManageCombatQueryHandler(ApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<ManageCombatQueryResponse> Handle(ManageCombatQuery request, CancellationToken cancellationToken)
    {
        var creatures = await _dbContext.Creatures
            .ProjectTo<ManageCombatQueryResponse.Creature>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return new ManageCombatQueryResponse
        {
            Creatures = creatures
        };
    }
}

public class ManageCombatQueryResponse
{
    public List<Creature> Creatures { get; set; } = new();

    public class Creature : ICreature
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required int HitDice { get; set; }
        public required string Size { get; set; }
        public required int DexPoints { get; set; }
        public required int ConPoints { get; set; }
        public required string Cr { get; set; }
    }
}

public class ManageCombatQueryResponseProfile : Profile
{
    public ManageCombatQueryResponseProfile()
    {
        CreateMap<ICreature, ManageCombatQueryResponse.Creature>();
    }
}