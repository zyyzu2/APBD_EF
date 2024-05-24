using APBD_EF.DTOs;
using APBD_EF.Models;

namespace APBD_EF.Services;

public interface ITripService
{
    public Task<List<TripDTO>> GetAllTrips();
    public Task<bool> AssignClientToTrip(int id, ClientTripDTO dto);
}