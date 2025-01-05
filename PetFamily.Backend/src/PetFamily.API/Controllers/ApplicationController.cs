using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Response;

namespace PetFamily.API.Controllers
{
    [Route("api/")]
    [ApiController]
    public abstract class ApplicationController : ControllerBase
    {
        public override OkObjectResult Ok(object? value)
        {
            var envelope = Envelope.Ok(value);

            return new OkObjectResult(envelope);
        }
    }
}
