using Microsoft.AspNetCore.Mvc;
using PetFamily.Framework.Response;

namespace PetFamily.Framework
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
