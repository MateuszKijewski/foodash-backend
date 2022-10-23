using FooDash.Application.Users.Dtos.Basic;
using FooDash.Domain.Entities.Identity;
using Mapster;

namespace FooDash.Application.MapperConfigurations
{
    public static class IdentityMapperConfiguration
    {
        public static TypeAdapterConfig AddIdentityConfiguration(this TypeAdapterConfig config)
        {
            config
                .NewConfig<UserDto, User>();
            config
                .NewConfig<User, ReadUserDto>()
                .IgnoreNullValues(true);

            return config;
        }
    }
}