using Microsoft.AspNetCore.Mvc;
using PetFamily.Accounts.Application.RegisterUser;
using PetFamily.Accounts.Contracts.Requests;
using PetFamily.Accounts.Presentation.Extensions;
using PetFamily.Framework;

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
}
 