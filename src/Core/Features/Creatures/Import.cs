using DMPowerTools.Core.Models.Imports;
using Microsoft.AspNetCore.Components.Forms;
using System.Text.Json;

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

public class ProcessMonsterFileProfile : Profile
{
    public ProcessMonsterFileProfile()
    {
        CreateMap<TetraCubeCreature, Creature>()
            .ForMember(c => c.ArmorClass, tcc => tcc.Ignore())
            .ForMember(c => c.Id, tcc => tcc.Ignore());
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
