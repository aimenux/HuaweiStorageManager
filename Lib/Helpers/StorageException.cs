using System;
using System.Runtime.Serialization;
using Lib.Configuration;
using OBS;

namespace Lib.Helpers
{
    [Serializable]
    public class StorageException : ApplicationException
    {
        protected StorageException()
        {
        }

        protected StorageException(string message) : base(message)
        {
        }

        protected StorageException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected StorageException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public static StorageException InternalStorageFailure(ObsException ex)
        {
            return new StorageException($"Internal storage failure (StatusCode = {ex.StatusCode})", ex);
        }

        public static StorageException FailedToGetBucketFileInfo(string bucketName, string fileName)
        {
            return new StorageException($"Failed to get info for file {fileName} of bucket {bucketName}");
        }

        public static StorageException FailedToListBucketFiles(string bucketName)
        {
            return new StorageException($"Failed to list files in bucket {bucketName}");
        }

        public static StorageException FailedToUploadBucketFile(string bucketName, string fileToUpload)
        {
            return new StorageException($"Failed to upload file {fileToUpload} to bucket {bucketName}");
        }

        public static StorageException FailedToDownloadBucketFile(string bucketName, string fileToDownload)
        {
            return new StorageException($"Failed to download file {fileToDownload} from bucket {bucketName}");
        }

        public static StorageException FailedToCopyBucketFile(string sourceFileName)
        {
            return new StorageException($"Failed to copy file {sourceFileName}");
        }

        public static StorageException FailedToDeleteBucketFile(string bucketName, string fileName)
        {
            return new StorageException($"Failed to delete file {fileName} from bucket {bucketName}");
        }

        public static StorageException FailedToFindBucketFile(string bucketName, string fileName)
        {
            return new StorageException($"Failed to find file {fileName} in bucket {bucketName}");
        }

        public static StorageException InvalidStorageSettings(StorageSettings settings)
        {
            return settings is null
                ? new StorageException("Storage settings section is not found")
                : new StorageException("Storage settings section is not valid");
        }
    }
}