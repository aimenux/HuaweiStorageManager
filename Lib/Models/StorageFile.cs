using System.Collections.Generic;
using Lib.Extensions;
using OBS.Model;

namespace Lib.Models
{
    public class StorageFile
    {
        public string Name { get; set; }

        public string Size { get; set; }

        public Date ModificationDate { get; set; }

        public IDictionary<string,string> Headers { get; set; }

        public StorageFile()
        {
        }

        public StorageFile(ObsObject obsObject)
        {
            Name = obsObject.ObjectKey;
            Size = obsObject.Size.FriendlySize();
            ModificationDate = obsObject.LastModified;
        }
    }
}
