using System.Text.Json;

namespace DMPowerTools.Core.Features.Settings;

public record ExportCreatureDataQuery : IRequest<ExportCreatureDataQueryResponse> { }

public class ExportCreatureDataHandler : IRequestHandler<ExportCreatureDataQuery, ExportCreatureDataQueryResponse>
{
    private readonly ApplicationDbContext _dbContext;

    public ExportCreatureDataHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ExportCreatureDataQueryResponse> Handle(ExportCreatureDataQuery query, CancellationToken cancellationToken)
    {
        var creatures = await _dbContext.Creatures.ToListAsync(cancellationToken);

        byte[] fileBytes;
        using (var memoryStream = new MemoryStream())
        {
            await JsonSerializer.SerializeAsync(memoryStream, creatures);
            fileBytes = memoryStream.ToArray();
        }

        var fileName = $"exported-creatures{Guid.NewGuid()}.json";

        return new ExportCreatureDataQueryResponse(fileBytes, fileName);
    }
}

public record ExportCreatureDataQueryResponse(byte[] ExportedCreatureFileBytes, string FileName) { }
