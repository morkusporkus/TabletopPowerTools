namespace TabletopPowerTools.Core.Features.Combat.Manage;

public class CreatureSearchQuery : IRequest<CreatureSearchQueryResponse> { }

public class CreatureSearchQueryHandler : IRequestHandler<CreatureSearchQuery, CreatureSearchQueryResponse>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    public CreatureSearchQueryHandler(ApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<CreatureSearchQueryResponse> Handle(CreatureSearchQuery request, CancellationToken cancellationToken)
    {
        var creatures = await _dbContext.Creatures
            .ProjectTo<CreatureSearchQueryResponse.Creature>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return new CreatureSearchQueryResponse
        {
            Creatures = creatures
        };
    }
}

public class CreatureSearchQueryResponse
{
    public List<Creature> Creatures { get; set; } = new();

    public class Creature : ICreature
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required int HitDice { get; set; }
        public required string Size { get; set; }
        public required AbilityScores AbilityScores { get; set; }
        public required string Cr { get; set; }
        public required int ArmorClass { get; set; }
    }
}

public class CreatureSearchQueryResponseProfile : Profile
{
    public CreatureSearchQueryResponseProfile()
    {
        CreateMap<ICreature, CreatureSearchQueryResponse.Creature>();
    }
}