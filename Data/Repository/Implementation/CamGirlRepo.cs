using Data.Context;
using Data.Repository.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Model.DTO;
using Model.Enitities;

namespace Data.Repository.Implementation
{
    public class CamGirlRepo : ICamGirlRepo
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly DatingSiteContext _context;

        public CamGirlRepo(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, DatingSiteContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        public async Task<PaginatedUser> GetCamGirlsAvailableAsync(int pageNumber, int perPageSize, CamgirlPreference camgirl)
        {
            pageNumber = pageNumber < 1 ? 1 : pageNumber;
            perPageSize = perPageSize < 1 ? 5 : perPageSize;

            var filteredCamgirl = _userManager.Users
                .Join(
                    _context.UserRoles,
                    user => user.Id,
                    userRole => userRole.UserId,
                    (user, userRole) => new { User = user, UserRole = userRole })
                .Join(
                    _roleManager.Roles,
                    userRole => userRole.UserRole.RoleId,
                    role => role.Id,
                    (userRole, role) => new { User = userRole.User, Role = role })
                .Where(u => u.Role.Name == "CAMGIRL"
                            && u.User.Location == camgirl.Location
                            && u.User.Age == camgirl.Age
                            && u.User.Gender == camgirl.Gender
                            && u.User.IsUserOnline
                            && !u.User.UserIsTaken)
                .Select(u => new DisplayFindUserDTO
                {
                    UserName = u.User.UserName,
                    Email = u.User.Email,
                    FirstName = u.User.FirstName,
                    LastName = u.User.LastName,
                    PhoneNumber = u.User.PhoneNumber,
                    ProfilePicture = u.User.ProfilePicture,
                    Gender = u.User.Gender,
                    Id = u.User.Id,
                    TimeAvailable = u.User.TimeAvailable
                });

            var totalCount = await filteredCamgirl.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalCount / perPageSize);

            var paginatedCamgirl = await filteredCamgirl
                .Skip((pageNumber - 1) * perPageSize)
                .Take(perPageSize)
                .ToListAsync();
            var result = new PaginatedUser
            {
                CurrentPage = pageNumber,
                PageSize = perPageSize,
                TotalPages = totalPages,
                User = paginatedCamgirl,
            };
            return result;
        }

        public async Task<PaginatedUser> GetAllCamGirlsAsync(int pageNumber, int perPageSize)
        {
            pageNumber = pageNumber < 1 ? 1 : pageNumber;
            perPageSize = perPageSize < 1 ? 5 : perPageSize;

            var filteredCamgirl = _userManager.Users
                .Join(
                    _context.UserRoles,
                    user => user.Id,
                    userRole => userRole.UserId,
                    (user, userRole) => new { User = user, UserRole = userRole })
                .Join(
                    _roleManager.Roles,
                    userRole => userRole.UserRole.RoleId,
                    role => role.Id,
                    (userRole, role) => new { User = userRole.User, Role = role })
                .Where(u => u.Role.Name == "CAMGIRL")
                .Select(u => new DisplayFindUserDTO
                {
                    UserName = u.User.UserName,
                    Email = u.User.Email,
                    FirstName = u.User.FirstName,
                    LastName = u.User.LastName,
                    PhoneNumber = u.User.PhoneNumber,
                    ProfilePicture = u.User.ProfilePicture,
                    Gender = u.User.Gender,
                    Id = u.User.Id,
                    TimeAvailable = u.User.TimeAvailable
                });

            var totalCount = await filteredCamgirl.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalCount / perPageSize);

            var paginatedCamgirl = await filteredCamgirl
                .Skip((pageNumber - 1) * perPageSize)
                .Take(perPageSize)
                .ToListAsync();
            var result = new PaginatedUser
            {
                CurrentPage = pageNumber,
                PageSize = perPageSize,
                TotalPages = totalPages,
                User = paginatedCamgirl,
            };
            return result;
        }

        public async Task<ApplicationUser> FindCamGirlbyUserName(string userName)
        {
            var findCamgirl = await _context.Users.FirstOrDefaultAsync(e => e.UserName == userName);
            return findCamgirl;
        }

        public async Task<bool> CheckInCamgirlRole(string username)
        {
            var filteredCamgirl = await _userManager.Users
                .Join(
                    _context.UserRoles,
                    user => user.Id,
                    userRole => userRole.UserId,
                    (user, userRole) => new { User = user, UserRole = userRole })
                .Join(
                    _roleManager.Roles,
                    userRole => userRole.UserRole.RoleId,
                    role => role.Id,
                    (userRole, role) => new { User = userRole.User, Role = role })
                .Where(u => u.Role.Name == "CAMGIRL"
                            && u.User.UserName == username
                            ).ToListAsync();
            if (filteredCamgirl.Any()) { return true; }
            return false;
        }
    }
}