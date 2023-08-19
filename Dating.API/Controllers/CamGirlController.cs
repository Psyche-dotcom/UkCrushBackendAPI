using Dating.API.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Model.DTO;
using Model.Enum;

namespace Dating.API.Controllers
{
    [Route("api/camgirl")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ApiController]
    public class CamGirlController : ControllerBase
    {
        private readonly ICamGirlService _camGirlService;
        private readonly IEmailServices _emailServices;
        private IMemoryCache _cache;
        private static readonly SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);

        public CamGirlController(ICamGirlService camGirlService, IEmailServices emailServices, IMemoryCache cache)
        {
            _camGirlService = camGirlService;
            _emailServices = emailServices;
            _cache = cache;
        }

        [HttpGet("available/{page_number}")]
        public async Task<IActionResult> GetCamGirlAvailableAsync(int page_number)
        {
           var result = await _camGirlService.GetCamGirlsAvailableAsync(page_number, 5);
            if (result.StatusCode == 200)
            {
                return Ok(result);
            }
            else if (result.StatusCode == 404)
            {
                return NotFound(result);
            }
            else
            {
                return BadRequest(result);
            }
           
        }

        [Authorize(AuthenticationSchemes = "Bearer", Roles = "ADMIN")]
        [HttpGet("all/{per_page_size}/{page_number}")]
        public async Task<IActionResult> GetAllCamGirlAsync(int page_number, int per_page_size)
        {
            var cacheKey = $"GetCamGirlAvailableAsync_{page_number}_{per_page_size}";
            if (_cache.TryGetValue(cacheKey, out ResponseDto<PaginatedUser> result))
            {
                return Ok(result);
            }
            else
            {
                try
                {
                    await semaphore.WaitAsync();
                    if (_cache.TryGetValue(cacheKey, out result))
                    {
                        return Ok(result);
                    }
                    else
                    {
                        var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromSeconds(120))
                        .SetAbsoluteExpiration(TimeSpan.FromSeconds(3600))
                        .SetPriority(CacheItemPriority.Normal)
                        .SetSize(1024);
                        result = await _camGirlService.GetAllCamGirlsAsync(page_number, per_page_size);
                        _cache.Set(cacheKey, result, cacheEntryOptions);
                        if (result.StatusCode == 200)
                        {
                            return Ok(result);
                        }
                        else if (result.StatusCode == 404)
                        {
                            return NotFound(result);
                        }
                        else
                        {
                            return BadRequest(result);
                        }
                    }
                }
                finally { semaphore.Release(); }
            }
        }

        [HttpPost("match")]
        public async Task<IActionResult> MatchCamgirl(MatchGirlDto matchGirl)
        {
            var result = await _camGirlService.SetCamgirlAsTaken(matchGirl.Email);
            try
            {
                await semaphore.WaitAsync();
                for (int pageNumber = 1; pageNumber <= 100; pageNumber++)
                {
                    
                    for (int pageSize = 1; pageSize <= 100; pageSize++)
                    {
                      var cacheKey = $"GetCamGirlAvailableAsync_{pageNumber}_{pageSize}";
                        _cache.Remove(cacheKey);
                    }
                }
            }
            finally { semaphore.Release(); }
            if (result.StatusCode == 200)
            {
                var message = new Message(new string[] { matchGirl.Email }, "Join Room Link", $"<p>Click <a href={matchGirl.RoomLink}>here</a> to join the client</p>");
                _emailServices.SendEmail(message);
                return Ok(result);
            }
            else if (result.StatusCode == 404)
            {
                return NotFound(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpPost("unmatch/{email}")]
        public async Task<IActionResult> UnMatchCamgirl(string email)
        {
            var result = await _camGirlService.SetCamgirlAsNotTaken(email);
            try
            {
                await semaphore.WaitAsync();
                for (int pageNumber = 1; pageNumber <= 100; pageNumber++)
                {
                    for (int pageSize = 1; pageSize <= 100; pageSize++)
                    {
                        var cacheKey = $"GetCamGirlAvailableAsync_{pageNumber}_{pageSize}";
                        _cache.Remove(cacheKey);
                    }
                }
            }
            finally { semaphore.Release(); }
            if (result.StatusCode == 200)
            {
                return Ok(result);
            }
            else if (result.StatusCode == 404)
            {
                return NotFound(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpGet("{username}")]
        public async Task<IActionResult> GetCamgirl(string username)
        {
            var cacheKey = $"GetCamgirl_{username}";
            if (_cache.TryGetValue(cacheKey, out ResponseDto<DisplayFindUserDTO> result))
            {
                return Ok(result);
            }
            else
            {
                try
                {
                    await semaphore.WaitAsync();
                    if (_cache.TryGetValue(cacheKey, out result))
                    {
                        return Ok(result);
                    }
                    else
                    {
                        var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromSeconds(120))
                        .SetAbsoluteExpiration(TimeSpan.FromSeconds(3600))
                        .SetPriority(CacheItemPriority.Normal)
                        .SetSize(1024);
                        result = await _camGirlService.FindCamGirlbyUserName(username);
                        _cache.Set(cacheKey, result, cacheEntryOptions);
                        if (result.StatusCode == 200)
                        {
                            return Ok(result);
                        }
                        else if (result.StatusCode == 404)
                        {
                            return NotFound(result);
                        }
                        else
                        {
                            return BadRequest(result);
                        }
                    }
                }
                finally { semaphore.Release(); }
            }
        }
    }
}