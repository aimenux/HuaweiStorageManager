namespace Lib.Models
{
    public class CopyStorageFile
    {
        public string SourceBucketName { get; set; }

        public string SourceFileName { get; set; }

        public string TargetBucketName { get; set; }

        public string TargetFileName { get; set; }
    }
}