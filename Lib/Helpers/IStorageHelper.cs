using System.Collections.Generic;
using Lib.Models;

namespace Lib.Helpers
{
    public interface IStorageHelper
    {
        DeleteStorageFile DeleteStorageFile(string bucketName, string fileName);

        StorageFile GetStorageFileInfo(string bucketName, string fileName);

        ICollection<StorageFile> ListStorageFiles(string bucketName, int maxItems);

        UploadStorageFile UploadStorageFile(string bucketName, string fileName, string uploadPath, string headersFile);

        DownloadStorageFile DownloadStorageFile(string bucketName, string fileName, string downloadPath);

        CopyStorageFile CopyStorageFile(string sourceBucketName, string sourceFileName, string targetBucketName, string targetFileName);
    }
}