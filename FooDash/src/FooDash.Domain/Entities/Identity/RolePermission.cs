using FooDash.Domain.Common.Attributes;
using FooDash.Domain.Common.Entities;

namespace FooDash.Domain.Entities.Identity
{
    [SystemEntity]
    public class RolePermission : EntityBase
    {
        public Guid RoleId { get; set; }
        public Role? Role { get; set; }
        public Guid PermissionId { get; set; }
        public Permission? Permission { get; set; }
    }
}