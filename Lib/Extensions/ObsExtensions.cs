using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using OBS.Model;

namespace Lib.Extensions
{
    public static class ObsExtensions
    {
        public static void AddHeaders(this PutObjectRequest request, string headersFile)
        {
            if (string.IsNullOrWhiteSpace(headersFile) || !File.Exists(headersFile))
            {
                return;
            }

            var content = File.ReadAllText(headersFile);
            var headers = JsonSerializer.Deserialize<Headers>(content);
            if (headers is null || !headers.Any())
            {
                return;
            }

            var metadata = new MetadataCollection();
            foreach (var header in headers)
            {
                metadata.Add(header.Name, header.Value);
            }

            var property = request.GetType().GetProperty(nameof(request.Metadata));
            property?.SetMethod?.Invoke(request, new object[] { metadata });
        }

        internal class Headers : List<Header>
        {
        }

        internal class Header
        {
            public string Name { get; set; }
            public string Value { get; set; }
        }
    }
}
