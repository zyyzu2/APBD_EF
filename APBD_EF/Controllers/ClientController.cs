using APBD_EF.Exceptions;
using APBD_EF.Services;
using Microsoft.AspNetCore.Mvc;

namespace APBD_EF.Controllers;

[Route("/api/clients")]
[ApiController]
public class ClientController(IClientService service) : ControllerBase
{
    private readonly IClientService _service = service;

    [HttpDelete]
    [Route("/{id}")]
    public async Task<IActionResult> DeleteClient([FromRoute] int id)
    {
        try
        {
            await _service.DeleteClient(id);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (AssignedTripException e)
        {
            return Conflict(e.Message);
        }
        return NoContent();
    }
}