using GingerCodeSite.Data.Access.Queries.Developers;
using GingerCodeSite.Data.Domain;
using Microsoft.Extensions.Logging;

namespace GingerCodeSite.Data.Services.Developers;

//todo: Remove this file! It's just to demonstrate what a data service might look like.

public class DeveloperQueries : IDataService
{
    private readonly ILogger< DeveloperQueries > _logger;
    private readonly RetrieveDeveloper _retrieveDeveloperQuery;

    public DeveloperQueries( RetrieveDeveloper retrieveDeveloperQuery , ILogger< DeveloperQueries > logger )
    {
        _retrieveDeveloperQuery = retrieveDeveloperQuery;
        _logger                 = logger;
    }

    public async Task< Developer? > RetrieveDeveloperAsync( Guid developerId , CancellationToken cancellationToken = default )
        => await _retrieveDeveloperQuery.ExecuteAsync( developerId , cancellationToken )
                                        .ConfigureAwait( false );
}
