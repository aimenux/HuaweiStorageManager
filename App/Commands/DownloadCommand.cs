using System.ComponentModel.DataAnnotations;
using System.IO;
using Lib.Helpers;
using McMaster.Extensions.CommandLineUtils;

namespace App.Commands
{
    [Command(Name = "Download", FullName = "Download files from buckets", Description = "Download files from buckets.")]
    [VersionOptionFromMember(MemberName = nameof(GetVersion))]
    [HelpOption]
    public class DownloadCommand : AbstractCommand
    {
        private readonly IStorageHelper _storageHelper;

        public DownloadCommand(IStorageHelper storageHelper, IConsoleHelper consoleHelper) : base(consoleHelper)
        {
            _storageHelper = storageHelper;
        }

        [Required]
        [Option("-b|--bucket", "BucketName", CommandOptionType.SingleValue)]
        public string BucketName { get; set; }

        [Required]
        [Option("-f|--file", "FileName", CommandOptionType.SingleValue)]
        public string FileName { get; set; }

        [Option("-p|--path", "DownloadPath", CommandOptionType.SingleValue)]
        public string DownloadPath { get; set; }

        protected override void Execute(CommandLineApplication _)
        {
            if (string.IsNullOrWhiteSpace(DownloadPath))
            {
                DownloadPath = Path.GetFullPath("./");
            }

            DownloadPath = Path.GetFullPath(DownloadPath);

            var storageFile = _storageHelper.DownloadStorageFile(BucketName, FileName, DownloadPath);
            ConsoleHelper.RenderStorageFile(storageFile);
        }

        protected override bool HasValidOptions()
        {
            return !string.IsNullOrWhiteSpace(DownloadPath) 
                   && Directory.Exists(Path.GetDirectoryName(DownloadPath));
        }

        private static string GetVersion() => GetVersion(typeof(DownloadCommand));
    }
}