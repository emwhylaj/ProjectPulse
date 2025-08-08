using System.ComponentModel.Design.Serialization;

namespace ProjectPulseAPI.Core.Entities
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string UpdatedBy { get; set; } = string.Empty;
        public bool? IsDeleted { get; set; }
        public string? DeletedBy { get; set; }
    }
}