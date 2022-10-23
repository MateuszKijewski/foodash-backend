namespace FooDash.Application.Auth.Dtos.Contracts
{
    public class SetRolePermissionsContract
    {
        public Guid RoleId { get; set; }
        public Guid[] PermissionIds { get; set; } = Array.Empty<Guid>();
    }
}