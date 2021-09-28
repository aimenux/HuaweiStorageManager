using System;
using System.Collections.Generic;
using Lib.Models;
using OBS;

namespace Lib.Helpers
{
    public interface IConsoleHelper
    {
        void RenderTitle(string text);

        void RenderSettingsFile(string filepath);

        void RenderStorageFiles(ICollection<StorageFile> storageFiles);

        void RenderStorageFile(StorageFile storageFile);

        void RenderStorageFile(UploadStorageFile storageFile);

        void RenderStorageFile(DownloadStorageFile storageFile);

        void RenderStorageFile(CopyStorageFile storageFile);

        void RenderStorageFile(DeleteStorageFile storageFile);

        void RenderException(Exception exception);

        void RenderException(ObsException exception);
    }
}
