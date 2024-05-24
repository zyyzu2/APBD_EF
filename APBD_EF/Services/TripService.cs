using APBD_EF.Context;
using APBD_EF.DTOs;
using APBD_EF.Exceptions;
using APBD_EF.Models;
using Microsoft.EntityFrameworkCore;

namespace APBD_EF.Services;

public class TripService : ITripService
{
    private readonly MasterContext _context;

    public TripService(MasterContext context)
    {
        _context = context;
    }
    public async Task<List<TripDTO>> GetAllTrips()
    {
        var trips = await _context.Trips
            .Include(t => t.ClientTrips)
            .Include(t => t.IdCountries)
            .Select(t => new TripDTO
            {
                Name = t.Name,
                Description = t.Description,
                DateFrom = t.DateFrom,
                DateTo = t.DateTo,
                MaxPeople = t.MaxPeople,
                Countries = t.IdCountries.Select(c => new CountryDTO
                {
                    Name = c.Name
                }),
                Clients = t.ClientTrips.Select(cl => new ClientDTO
                {
                    FirstName = cl.IdClientNavigation.FirstName,
                    LastName = cl.IdClientNavigation.LastName
                })
            })
            .OrderByDescending(t => t.DateFrom)
            .ToListAsync();
        return trips;
    }

    public async Task<bool> AssignClientToTrip(int id, ClientTripDTO dto)
    {
        if (id != dto.IdTrip) throw new BadHttpRequestException("Route id missmatch");
        var client = await _context.Clients.FirstOrDefaultAsync(cl => cl.Pesel == dto.Pesel);
        if (client is null)
        {
            client = new Client()
            {
                IdClient = await _context.Clients.MaxAsync(c => c.IdClient) + 1,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Telephone = dto.Telephone,
                Pesel = dto.Pesel
            };
            await _context.Clients.AddAsync(client);
        }

        if (await _context.ClientTrips.FirstOrDefaultAsync(ct => ct.IdClientNavigation.Pesel == dto.Pesel) is not null)
            throw new AlreadyAssignedToTripException("Provided user is already assigned to this trip");
        if (await _context.Trips.FirstOrDefaultAsync(t => t.IdTrip == dto.IdTrip) is null)
            throw new NotFoundException("Provided trip does not exists");
        var clientTrip = new ClientTrip()
        {
            IdClient = client.IdClient,
            IdTrip = dto.IdTrip,
            RegisteredAt = DateTime.Now,
            PaymentDate = dto.PaymentDate
        };
        await _context.ClientTrips.AddAsync(clientTrip);
        await _context.SaveChangesAsync();
        return true;
    }
}