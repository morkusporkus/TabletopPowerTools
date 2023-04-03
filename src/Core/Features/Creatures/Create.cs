using TabletopPowerTools.Core.Models.ViewModels;
using Action = TabletopPowerTools.Core.Models.Action;

namespace TabletopPowerTools.Core.Features.Creatures;
public class CreateCreatureCommand : IRequest<bool>
{
    public required CreatureViewModel Creature { get; set; }
}
public class CreateCreatureCommandHandler : IRequestHandler<CreateCreatureCommand, bool>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    public CreateCreatureCommandHandler(ApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<bool> Handle(CreateCreatureCommand request, CancellationToken cancellationToken)
    {
        var creature = _mapper.Map<Creature>(request.Creature);
        _dbContext.Add(creature);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }
}
public class ViewModelCreatureProfile : Profile
{
    public ViewModelCreatureProfile()
    {
        CreateMap<CreatureViewModel, Creature>()
            .ForMember(d => d.Id, mo => mo.Ignore())
            .ForMember(d=>d.Skills, mo=>mo.Ignore());
        CreateMap<AbilityViewModel, Ability>()
            .ForMember(d => d.Id, mo => mo.Ignore())
            .ForMember(d => d.CreatureId, mo => mo.Ignore());
        CreateMap<ActionViewModel, Action>()
            .ForMember(d => d.Id, mo => mo.Ignore())
            .ForMember(d => d.CreatureId, mo => mo.Ignore());
    }
}