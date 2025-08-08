namespace ProjectPulseAPI.Core.Entities
{
    public class ProjectFile : BaseEntity
    {
        public int ProjectId { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string OriginalFileName { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public string MimeType { get; set; } = string.Empty;
        public long FileSize { get; set; }
        public int UploadedBy { get; set; }

        // Navigation Properties
        public Project Project { get; set; } = null!;

        public User UploadedByUser { get; set; } = null!;
    }
}