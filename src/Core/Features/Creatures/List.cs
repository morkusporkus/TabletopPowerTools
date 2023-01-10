namespace DMPowerTools.Core.Features.Creatures;

public class ListQuery : IRequest<ListQueryResponse> { }

public class ListQueryHandler : IRequestHandler<ListQuery, ListQueryResponse>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    public ListQueryHandler(ApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<ListQueryResponse> Handle(ListQuery request, CancellationToken cancellationToken)
    {
        var creatures = await _dbContext.Creatures
            .ProjectTo<ListQueryResponse.Creature>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return new ListQueryResponse
        {
            Creatures = creatures
        };
    }
}

public class ListQueryResponse
{
    public List<Creature> Creatures { get; set; } = new();

    public class Creature
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required string Cr { get; set; }
        public required int HitPoints { get; set; }
    }
}

public class ListQueryResponseProfile : Profile
{
    public ListQueryResponseProfile()
    {
        CreateMap<Creature, ListQueryResponse.Creature>();
    }
}

public class DeleteCommand : IRequest<Unit>
{
    public int Id { get; set; }
}

public class DeleteCommandHandler : IRequestHandler<DeleteCommand, Unit>
{
    private readonly ApplicationDbContext _dbContext;

    public DeleteCommandHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Unit> Handle(DeleteCommand request, CancellationToken cancellationToken)
    {
        await _dbContext.Creatures
            .Where(c => c.Id == request.Id)
            .ExecuteDeleteAsync(cancellationToken);

        return Unit.Value;
    }
}