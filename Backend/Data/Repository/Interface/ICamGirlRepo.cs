using Model.DTO;
using Model.Enitities;

namespace Data.Repository.Interface
{
    public interface ICamGirlRepo
    {
        Task<PaginatedUser> GetCamGirlsAvailableAsync(int pageNumber, int perPageSize, CamgirlPreference camgirl);

        Task<ApplicationUser> FindCamGirlbyUserName(string userName);

        Task<bool> CheckInCamgirlRole(string username);

        Task<PaginatedUser> GetAllCamGirlsAsync(int pageNumber, int perPageSize);
    }
}