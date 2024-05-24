using APBD_EF.Context;
using APBD_EF.DTOs;
using APBD_EF.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APBD_EF.Controllers;

[Route("api/trips")]
[ApiController]
public class TripController : ControllerBase
{
    private readonly ITripService _service;
    
    public TripController(ITripService service)
    {
        _service = service;
    }
    
    
    [HttpGet]
    public async Task<IActionResult> GetAllTrips()
    {
        return Ok(await _service.GetAllTrips());
    }

    [HttpPost]
    [Route("/{id}/clients")]
    public async Task<IActionResult> AssignClientToTrip([FromRoute] int id, ClientTripDTO dto)
    {
        try
        {
            await _service.AssignClientToTrip(id, dto);
        }
        catch (Exception e)
        {
            return BadRequest();
        }
        return NoContent();
    }
}