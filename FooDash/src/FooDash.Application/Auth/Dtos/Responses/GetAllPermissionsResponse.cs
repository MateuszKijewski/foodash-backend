using FooDash.Domain.Entities.Identity;

namespace FooDash.Application.Auth.Dtos.Responses
{
    public class GetAllPermissionsResponse
    {
        public Guid PermissionId { get; set; }
        public PermissionType PermissionType { get; set; }
        public string PermissionName { get; set; }
        public BasicEntityInfo Entity { get; set; }
    }

    public class BasicEntityInfo
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}