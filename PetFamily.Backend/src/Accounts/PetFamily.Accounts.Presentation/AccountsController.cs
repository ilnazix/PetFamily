using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetFamily.Accounts.Application.Commands.Login;
using PetFamily.Accounts.Application.Commands.RegisterUser;
using PetFamily.Accounts.Contracts.Requests;
using PetFamily.Accounts.Presentation.Extensions;
using PetFamily.Framework;
using PetFamily.Framework.Auth;

namespace PetFamily.Accounts.Presentation;

[Route("[controller]")]
public class AccountsController : ApplicationController
{
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
        var result = await handler.Handle(request.ToCommand());

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HasPermission("123")]
    [HttpGet("protected")]
    public ActionResult TestAuth()
    {
        return Ok("authorized");
    }
}
 