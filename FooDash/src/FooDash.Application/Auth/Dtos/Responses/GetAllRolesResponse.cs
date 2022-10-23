using FooDash.Domain.Entities.Identity;

namespace FooDash.Application.Auth.Dtos.Responses
{
    public class GetAllRolesResponse
    {
        public Guid RoleId { get; set; }
        public string RoleName { get; set; }
        public IEnumerable<BasicPermissionInfo> Permissions { get; set; }
    }

    public class BasicPermissionInfo
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public PermissionType Type { get; set; }
    }
}