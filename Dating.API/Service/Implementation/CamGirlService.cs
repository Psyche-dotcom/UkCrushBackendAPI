using AutoMapper;
using Data.Repository.Interface;
using Dating.API.Service.Interface;
using Model.DTO;

namespace Dating.API.Service.Implementation
{
    public class CamGirlService : ICamGirlService
    {
        private readonly ICamGirlRepo _camGirlRepo;
        private readonly ILogger<CamGirlService> _logger;
        private readonly IAccountRepo _accountRepo;
        private readonly IMapper _mapper;

        public CamGirlService(ICamGirlRepo camGirlRepo, ILogger<CamGirlService> logger, IAccountRepo accountRepo, IMapper mapper)
        {
            _camGirlRepo = camGirlRepo;
            _logger = logger;
            _accountRepo = accountRepo;
            _mapper = mapper;
        }

        public async Task<ResponseDto<PaginatedUser>> GetCamGirlsAvailableAsync(int pageNumber, int perPageSize, CamgirlPreference camgirl)
        {
            var response = new ResponseDto<PaginatedUser>();
            try
            {
                var getCAMGIRL = await _camGirlRepo.GetCamGirlsAvailableAsync(pageNumber, perPageSize, camgirl);
                if (!getCAMGIRL.User.Any())
                {
                    response.ErrorMessages = new List<string>() { "Cam girl not available with the preference provided" };
                    response.StatusCode = 404;
                    response.DisplayMessage = "Error";
                    return response;
                }
                response.StatusCode = StatusCodes.Status200OK;
                response.DisplayMessage = "Success";
                response.Result = getCAMGIRL;
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                response.ErrorMessages = new List<string>() { "Error in getting camgirl  role" };
                response.StatusCode = 500;
                response.DisplayMessage = "Error";
                return response;
            }
        }

        public async Task<ResponseDto<PaginatedUser>> GetAllCamGirlsAsync(int pageNumber, int perPageSize)
        {
            var response = new ResponseDto<PaginatedUser>();
            try
            {
                var getCAMGIRL = await _camGirlRepo.GetAllCamGirlsAsync(pageNumber, perPageSize);
                response.StatusCode = StatusCodes.Status200OK;
                response.DisplayMessage = "Success";
                response.Result = getCAMGIRL;
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                response.ErrorMessages = new List<string>() { "Error in retrieving all camgirl" };
                response.StatusCode = 500;
                response.DisplayMessage = "Error";
                return response;
            }
        }

        public async Task<ResponseDto<string>> SetCamgirlAsTaken(string camGirlEmail)
        {
            var response = new ResponseDto<string>();
            try
            {
                var findUser = await _accountRepo.FindUserByEmailAsync(camGirlEmail);
                if (findUser == null)
                {
                    response.ErrorMessages = new List<string>() { "There is no user with the email provided" };
                    response.StatusCode = 404;
                    response.DisplayMessage = "Error";
                    return response;
                }
                if (findUser.UserIsTaken == true)
                {
                    response.ErrorMessages = new List<string>() { "Cam girl is taken, cam girl not available" };
                    response.StatusCode = 400;
                    response.DisplayMessage = "Error";
                    return response;
                }
                var setCamGirlHasTaken = await _accountRepo.UpdateUserStatusTaken(findUser);
                if (setCamGirlHasTaken == false)
                {
                    response.ErrorMessages = new List<string>() { "Error in matching cam girl" };
                    response.StatusCode = 400;
                    response.DisplayMessage = "Error";
                    return response;
                }
                response.StatusCode = StatusCodes.Status200OK;
                response.DisplayMessage = "Success";
                response.Result = "Cam girl successfully matched";
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                response.ErrorMessages = new List<string>() { "Error in Connecting Cam Girl" };
                response.StatusCode = 500;
                response.DisplayMessage = "Error";
                return response;
            }
        }

        public async Task<ResponseDto<string>> SetCamgirlAsNotTaken(string camGirlEmail)
        {
            var response = new ResponseDto<string>();
            try
            {
                var findUser = await _accountRepo.FindUserByEmailAsync(camGirlEmail);
                if (findUser == null)
                {
                    response.ErrorMessages = new List<string>() { "There is no cam girl with the email provided" };
                    response.StatusCode = 404;
                    response.DisplayMessage = "Error";
                    return response;
                }

                var setCamGirlHasTaken = await _accountRepo.UpdateUserStatusNotTaken(findUser);
                if (setCamGirlHasTaken == false)
                {
                    response.ErrorMessages = new List<string>() { "Error dismatching cam girl" };
                    response.StatusCode = 400;
                    response.DisplayMessage = "Error";
                    return response;
                }
                response.StatusCode = StatusCodes.Status200OK;
                response.DisplayMessage = "Success";
                response.Result = "Cam girl successfully dismatched";
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                response.ErrorMessages = new List<string>() { "Error in disconnecting Cam Girl" };
                response.StatusCode = 500;
                response.DisplayMessage = "Error";
                return response;
            }
        }

        public async Task<ResponseDto<DisplayFindUserDTO>> FindCamGirlbyUserName(string userName)
        {
            var response = new ResponseDto<DisplayFindUserDTO>();
            try
            {
                var findCamgirl = await _camGirlRepo.FindCamGirlbyUserName(userName);
                if (findCamgirl == null)
                {
                    response.ErrorMessages = new List<string>() { "Cam girl not available with the preference provided" };
                    response.StatusCode = 404;
                    response.DisplayMessage = "Error";
                    return response;
                }
                var checkInCamgirlRole = await _camGirlRepo.CheckInCamgirlRole(userName);
                if (checkInCamgirlRole == false)
                {
                    response.ErrorMessages = new List<string>() { "username provided is not a camgirl" };
                    response.StatusCode = 400;
                    response.DisplayMessage = "Error";
                    return response;
                }
                var mapCamGirl = _mapper.Map<DisplayFindUserDTO>(findCamgirl);
                response.StatusCode = StatusCodes.Status200OK;
                response.DisplayMessage = "Success";
                response.Result = mapCamGirl;
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                response.ErrorMessages = new List<string>() { "Error in getting camgirl by its username" };
                response.StatusCode = 500;
                response.DisplayMessage = "Error";
                return response;
            }
        }
    }
}