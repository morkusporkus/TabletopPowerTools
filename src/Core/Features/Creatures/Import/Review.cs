namespace TabletopPowerTools.Core.Features.Creatures.Import;

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
