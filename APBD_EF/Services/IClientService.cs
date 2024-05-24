namespace APBD_EF.Services;

public interface IClientService
{
    public Task<bool> DeleteClient(int id);
}