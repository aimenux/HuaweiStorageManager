using System.Collections.Generic;
using Lib.Models;

namespace Lib.Helpers
{
    public interface IStorageHelper
    {
        StorageFile GetStorageFileInfo(string bucketName, string fileName);

        ICollection<StorageFile> ListStorageFiles(string bucketName, int maxItems);

        UploadStorageFile UploadStorageFile(string bucketName, string fileName, string uploadPath);

        DownloadStorageFile DownloadStorageFile(string bucketName, string fileName, string downloadPath);
    }
}