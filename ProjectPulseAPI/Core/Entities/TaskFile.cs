namespace ProjectPulseAPI.Core.Entities
{
    public class TaskFile : BaseEntity
    {
        public int TaskId { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string OriginalFileName { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public string MimeType { get; set; } = string.Empty;
        public long FileSize { get; set; }
        public int UploadedBy { get; set; }

        // Navigation Properties
        public UserTask Task { get; set; } = null!;

        public User UploadedByUser { get; set; } = null!;
    }
}