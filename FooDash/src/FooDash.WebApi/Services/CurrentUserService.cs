using Dawn;
using FooDash.Application.Common.Interfaces.Auth;
using FooDash.Application.Common.Interfaces.Repositories;
using FooDash.Domain.Entities.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;

namespace FooDash.WebApi.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserRepository _userRepository;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor, IUserRepository userRepository)
        {
            _httpContextAccessor = Guard.Argument(httpContextAccessor).NotNull().Value;
            _userRepository = Guard.Argument(userRepository).NotNull().Value;
        }

        public Guid Id => User.Id;
        public string Email => User.Email;
        public string FirstName => User.FirstName;
        public string LastName => User.LastName;
        public string Name => User.Name;

        public Guid? RoleId => User.RoleId;
        public Guid LanguageId => User.LanguageId;
        public Guid CurrencyId => User.CurrencyId;

        public User User => GetCurrentUser();

        private User? GetCurrentUser()
        {
            var context = _httpContextAccessor.HttpContext;
            if (!context.Items.ContainsKey(nameof(User)))
            {
                var user = context.User;
                if (user == null)
                    return null;

                var email = user.Claims.SingleOrDefault(x => x.Type == JwtRegisteredClaimNames.UniqueName);
                if (email == null || string.IsNullOrEmpty(email.Value))
                    return null;

                context.Items[nameof(User)] = (User?)_userRepository.GetByEmail(email.Value);
            }

            return context.Items[nameof(User)] as User;
        }
    }
}
