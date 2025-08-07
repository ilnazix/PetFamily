using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetFamily.Framework;

namespace PetFamily.Accounts.Presentation;

[Route("[controller]")]
public class AccountsController : ApplicationController
{
    [Authorize]
    [HttpGet("secret")]
    public ActionResult TestAuth()
    {
        return Ok();
    }
}
