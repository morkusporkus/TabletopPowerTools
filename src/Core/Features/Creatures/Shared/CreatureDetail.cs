namespace DMPowerTools.Core.Features.Creatures.Shared;

public class CreatureDetailQuery : IRequest<CreatureDetailQueryResponse>
{
    public required int Id { get; set; }
}

public class CreatureDetailQueryHandler : IRequestHandler<CreatureDetailQuery, CreatureDetailQueryResponse>
{
    private readonly ApplicationDbContext _dbContext;

    public CreatureDetailQueryHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<CreatureDetailQueryResponse> Handle(CreatureDetailQuery request, CancellationToken cancellationToken)
    {
        var creature = await _dbContext.Creatures
            .FindAsync(new object[] { request.Id }, cancellationToken);

        return new CreatureDetailQueryResponse
        {
            Creature = creature!
        };
    }
}

// TODO: Map from/to a model not the database model.
public class CreatureDetailQueryResponse
{
    public Creature Creature { get; set; } = new();
}