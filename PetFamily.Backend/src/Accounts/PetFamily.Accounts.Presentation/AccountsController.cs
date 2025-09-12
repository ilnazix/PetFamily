using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PetFamily.Accounts.Application.Commands.Login;
using PetFamily.Accounts.Application.Commands.RefreshToken;
using PetFamily.Accounts.Application.Commands.RegisterUser;
using PetFamily.Accounts.Contracts.Requests;
using PetFamily.Accounts.Presentation.Extensions;
using PetFamily.Framework;
using PetFamily.Accounts.Infrastructure.Options.RefreshSession;
using CSharpFunctionalExtensions;
using PetFamily.Accounts.Application.Commands;
using PetFamily.SharedKernel;
using System.Linq.Dynamic.Core.Tokenizer;

namespace PetFamily.Accounts.Presentation;

[Route("[controller]")]
public class AccountsController : ApplicationController
{
    private const string REFRESH_TOKEN_COOKIE = "refreshToken";
    private readonly RefreshSessionOptions _refreshSessionOptions;

    public AccountsController(IOptions<RefreshSessionOptions> options)
    {
        _refreshSessionOptions = options.Value;
    }

    [HttpPost("registration")]
    public async Task<ActionResult> Register( 
        [FromBody] RegisterUserRequest request,
        [FromServices] RegisterUserCommandHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request.ToCommand());

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Created();
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login(
        [FromBody] LoginUserRequest request,
        [FromServices] LoginUserCommandHandler handler,
        CancellationToken cancellationToken)
    {
        var metadata = HttpContext.Request.GetLoginMetadata(request.Fingerprint);
        var result = await handler.Handle(request.ToCommand(metadata));

        if (result.IsFailure)
            return result.Error.ToResponse();
        
        SetRefreshTokenInCookie(result);

        return Ok(result.Value);
    }

    [HttpPost("refresh")]
    public async Task<ActionResult> Refresh(
        [FromBody] RefreshTokenRequest request,
        [FromServices] RefreshTokenCommandHandler handler,
        CancellationToken cancellationToken)
    {
        HttpContext.Request.Cookies.TryGetValue(REFRESH_TOKEN_COOKIE, out var refreshToken);

        if (string.IsNullOrEmpty(refreshToken))
            return Unauthorized("Refresh token is missing");

        var metadata = HttpContext.Request.GetLoginMetadata(request.Fingerprint);
        var command = new RefreshTokenCommand(refreshToken, metadata);
        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        SetRefreshTokenInCookie(result);

        return Ok(result.Value);
    }

    private void SetRefreshTokenInCookie(Result<LoginUserResponse, ErrorList> result)
    {
        var tokens = result.Value;

        HttpContext.Response
            .Cookies
            .SetHttpOnlyCookie("refreshToken", tokens.RefreshToken, _refreshSessionOptions.RefreshTokenLifetimeInDays);
    }
}
 