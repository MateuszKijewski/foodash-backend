using FooDash.Domain.Entities.Identity;

namespace FooDash.Domain.Common.Entities
{
    public abstract class EntityBase
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? DisplayName { get; set; }
        public DateTime? CreatedOn { get; set; }
        public Guid? CreatorId { get; set; }
        public User? Creator { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public Guid? ModifierId { get; set; }
        public User? Modifier { get; set; }
    }
}