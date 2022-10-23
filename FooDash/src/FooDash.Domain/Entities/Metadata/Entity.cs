using FooDash.Domain.Common.Attributes;
using FooDash.Domain.Common.Entities;

namespace FooDash.Domain.Entities.Metadata
{
    [SystemEntity]
    public class Entity : EntityBase
    {
        public Guid CreatePermissionKey { get; set; }
        public Guid ReadPermissionKey { get; set; }
        public Guid UpdatePermissionKey { get; set; }
        public Guid DeletePermissionKey { get; set; }
        public bool IsTranslated { get; set; }
    }
}