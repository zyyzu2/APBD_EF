using APBD_EF.Context;
using APBD_EF.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace APBD_EF.Services;

public class ClientService(MasterContext _context) : IClientService
{

    public async Task<bool> DeleteClient(int id)
    {
        var client = await _context.Clients.FindAsync(id);
        if (client is null) throw new NotFoundException("Cannot find client with provided ID");
        if (await _context.ClientTrips.FirstOrDefaultAsync(ct => ct.IdClient == id) is not null) throw new AssignedTripException("Cannot delete user with assigned trip");
        await _context.Clients.Where(s => s.IdClient == id).ExecuteDeleteAsync();
        return true;
    }
}