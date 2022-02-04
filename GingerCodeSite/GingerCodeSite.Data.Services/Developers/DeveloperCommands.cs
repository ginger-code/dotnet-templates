using GingerCodeSite.Data.Access.Commands.Developers;
using GingerCodeSite.Data.Domain;
using Microsoft.Extensions.Logging;

namespace GingerCodeSite.Data.Services.Developers;

//todo: Remove this file! It's just to demonstrate what a data service might look like.

public class DeveloperCommands : IDataService
{
    private readonly InsertDeveloper _insertDeveloperCommand;
    private readonly ILogger< DeveloperCommands > _logger;

    public DeveloperCommands( InsertDeveloper insertDeveloperCommand , ILogger< DeveloperCommands > logger )
    {
        _insertDeveloperCommand = insertDeveloperCommand;
        _logger                 = logger;
    }

    public async Task InsertDeveloperAsync( Developer developer , CancellationToken cancellationToken = default )
    {
        await _insertDeveloperCommand.ExecuteAsync( developer , cancellationToken )
                                     .ConfigureAwait( false );
    }
}
