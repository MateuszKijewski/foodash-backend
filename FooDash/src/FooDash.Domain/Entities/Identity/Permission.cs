using FooDash.Domain.Common.Attributes;
using FooDash.Domain.Common.Entities;
using FooDash.Domain.Entities.Metadata;

namespace FooDash.Domain.Entities.Identity
{
    [SystemEntity]
    public class Permission : EntityBase
    {
        public PermissionType PermissionType { get; set; }
        public Guid EntityId { get; set; }
        public Entity? Entity { get; set; }
    }

    public enum PermissionType
    {
        Create,
        Read,
        Update,
        Delete
    }
}