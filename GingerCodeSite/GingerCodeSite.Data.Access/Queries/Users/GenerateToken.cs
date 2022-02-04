using GingerCodeSite.Data.Context;
using GingerCodeSite.Util;
using JWT.Algorithms;
using JWT.Builder;
using Microsoft.Extensions.Configuration;

namespace GingerCodeSite.Data.Access.Queries.Users;

/// <summary>
///     Generates a user-specific token
/// </summary>
public sealed class GenerateToken : ContextQuery< GenerateTokenRequest , GenerateTokenResponse >
{
    private readonly IConfigurationRoot _configuration;

    public GenerateToken( IConfigurationRoot configuration , GingerCodeSiteContext context )
        : base( context )
    {
        _configuration = configuration;
    }

    public override Task< GenerateTokenResponse > ExecuteAsync( GenerateTokenRequest input , CancellationToken cancellationToken = default )
        => Task.FromResult(
            new GenerateTokenResponse(
                JwtBuilder.Create()
                          .WithAlgorithm( new HMACSHA256Algorithm() )
                          .WithSecret( _configuration.GetJwtSecret() )
                          .Issuer( _configuration.GetJwtIssuer() )
                          .Audience( _configuration.GetJwtAudience() )
                          .AddClaim(
                              "exp" ,
                              DateTimeOffset.UtcNow.AddHours( 1 )
                                            .ToUnixTimeSeconds()
                          )
                          .AddClaim( ClaimName.FullName , input.Username )
                          .Encode()
            )
        );
}
