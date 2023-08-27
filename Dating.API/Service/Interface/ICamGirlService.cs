using Model.DTO;

namespace Dating.API.Service.Interface
{
    public interface ICamGirlService
    {
        Task<ResponseDto<PaginatedUser>> GetCamGirlsAvailableAsync(int pageNumber, int perPageSize);

        Task<ResponseDto<string>> SetCamgirlAsTaken(string camGirlEmail);

        Task<ResponseDto<string>> SetCamgirlAsNotTaken(string camGirlUserName);

        Task<ResponseDto<PaginatedUser>> GetAllCamGirlsAsync(int pageNumber, int perPageSize);

        Task<ResponseDto<DisplayFindUserDTO>> FindCamGirlbyUserName(string userName);
    }
}