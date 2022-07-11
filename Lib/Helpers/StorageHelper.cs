using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Lib.Configuration;
using Lib.Extensions;
using Lib.Models;
using Microsoft.Extensions.Options;
using OBS;
using OBS.Model;

namespace Lib.Helpers
{
    public class StorageHelper : IStorageHelper
    {
        private readonly StorageSettings _settings;
        private readonly IStorageSettingsValidator _validator;

        public StorageHelper(IOptions<StorageSettings> options, IStorageSettingsValidator validator)
        {
            _validator = validator;
            _settings = options.Value;
        }

        public DeleteStorageFile DeleteStorageFile(string bucketName, string fileName)
        {
            var client = GetClient();
            var headRequest = new HeadObjectRequest
            {
                BucketName = bucketName,
                ObjectKey = fileName
            };

            var exists = client.HeadObject(headRequest);
            if (!exists)
            {
                throw StorageException.FailedToFindBucketFile(bucketName, fileName);
            }

            var deleteRequest = new DeleteObjectRequest
            {
                BucketName = bucketName,
                ObjectKey = fileName
            };

            using var deleteResponse = client.DeleteObject(deleteRequest);
            if (!IsNoContentStatusCode(deleteResponse.StatusCode))
            {
                throw StorageException.FailedToDeleteBucketFile(bucketName, fileName);
            }

            return new DeleteStorageFile
            {
                BucketName = bucketName,
                FileName = fileName
            };
        }

        public StorageFile GetStorageFileInfo(string bucketName, string fileName)
        {
            var client = GetClient();
            var request = new GetObjectMetadataRequest
            {
                BucketName = bucketName,
                ObjectKey = fileName
            };

            using var response = client.GetObjectMetadata(request);
            if (!IsSuccessfulStatusCode(response.StatusCode))
            {
                throw StorageException.FailedToGetBucketFileInfo(bucketName, fileName);
            }

            return new StorageFile
            {
                Name = fileName,
                Headers = response.Headers,
                ModificationDate = response.LastModified,
                Size = response.ContentLength.FriendlySize()
            };
        }

        public ICollection<StorageFile> ListStorageFiles(string bucketName, int maxItems)
        {
            var client = GetClient();
            var request = new ListObjectsRequest
            {
                BucketName = bucketName
            };

            using var response = client.ListObjects(request);
            if (!IsSuccessfulStatusCode(response.StatusCode))
            {
                throw StorageException.FailedToListBucketFiles(bucketName);
            }

            var storageObjects = response.ObsObjects
                .Select(x => new StorageFile(x))
                .OrderByDescending(x => x.ModificationDate)
                .Take(maxItems)
                .ToList();

            return storageObjects;
        }

        public UploadStorageFile UploadStorageFile(string bucketName, string fileName, string uploadPath, string headersFile)
        {
            var client = GetClient();
            var request = new PutObjectRequest
            {
                BucketName = bucketName,
                ObjectKey = fileName,
                FilePath = uploadPath
            };

            request.AddHeaders(headersFile);

            using var response = client.PutObject(request);
            if (!IsSuccessfulStatusCode(response.StatusCode))
            {
                throw StorageException.FailedToUploadBucketFile(bucketName, fileName);
            }

            return new UploadStorageFile
            {
                BucketName = bucketName,
                FileName = fileName,
                UploadPath = uploadPath
            };
        }

        public DownloadStorageFile DownloadStorageFile(string bucketName, string fileName, string downloadPath)
        {
            var client = GetClient();
            var request = new GetObjectRequest
            {
                BucketName = bucketName,
                ObjectKey = fileName
            };

            using var response = client.GetObject(request);
            if (!IsSuccessfulStatusCode(response.StatusCode))
            {
                throw StorageException.FailedToDownloadBucketFile(bucketName, fileName);
            }

            var fullDownloadPath = GetDownloadPath(fileName, downloadPath);
            response.WriteResponseStreamToFile(fullDownloadPath);
            return new DownloadStorageFile
            {
                BucketName = bucketName,
                FileName = fileName,
                DownloadPath = fullDownloadPath
            };
        }

        public CopyStorageFile CopyStorageFile(string sourceBucketName, string sourceFileName, string targetBucketName, string targetFileName)
        {
            var client = GetClient();
            var request = new CopyObjectRequest
            {
                SourceBucketName = sourceBucketName,
                SourceObjectKey = sourceFileName,
                BucketName = targetBucketName,
                ObjectKey = targetFileName
            };

            using var response = client.CopyObject(request);
            if (!IsSuccessfulStatusCode(response.StatusCode))
            {
                throw StorageException.FailedToCopyBucketFile(sourceFileName);
            }

            return new CopyStorageFile
            {
                SourceBucketName = sourceBucketName,
                SourceFileName = sourceFileName,
                TargetBucketName = targetBucketName,
                TargetFileName = targetFileName
            };
        }

        private ObsClient GetClient()
        {
            if (!HasValidStorageSettings())
            {
                throw StorageException.InvalidStorageSettings(_settings);
            }

            var client = new ObsClient(_settings.AccessKey, _settings.SecretKey, _settings.Endpoint);
            return client;
        }

        private static bool IsSuccessfulStatusCode(HttpStatusCode statusCode)
        {
            return statusCode == HttpStatusCode.OK;
        }

        private static bool IsNoContentStatusCode(HttpStatusCode statusCode)
        {
            return statusCode == HttpStatusCode.NoContent;
        }

        private bool HasValidStorageSettings() => _validator.Validate(_settings).IsValid;

        private static string GetDownloadPath(string bucketFileName, string downloadFilePath)
        {
            if (IsDirectory(downloadFilePath))
            {
                return Path.Combine(downloadFilePath, bucketFileName);
            }

            var downloadFile = Path.GetFileName(downloadFilePath);
            var downloadDirectory = Path.GetDirectoryName(downloadFilePath) ?? "./";
            return Path.Combine(downloadDirectory, downloadFile);
        }

        private static bool IsDirectory(string path)
        {
            try
            {
                var attributes = File.GetAttributes(path);
                return attributes.HasFlag(FileAttributes.Directory);
            }
            catch
            {
                return false;
            }
        }
    }
}