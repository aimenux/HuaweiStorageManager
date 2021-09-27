using System.ComponentModel.DataAnnotations;
using System.IO;
using Lib.Helpers;
using McMaster.Extensions.CommandLineUtils;

namespace App.Commands
{
    [Command(Name = "Upload", FullName = "Upload files to buckets", Description = "Upload files to buckets.")]
    [VersionOptionFromMember(MemberName = nameof(GetVersion))]
    [HelpOption]
    public class UploadCommand : AbstractCommand
    {
        private readonly IStorageHelper _storageHelper;

        public UploadCommand(IStorageHelper storageHelper, IConsoleHelper consoleHelper) : base(consoleHelper)
        {
            _storageHelper = storageHelper;
        }

        [Required]
        [Option("-b|--bucket", "BucketName", CommandOptionType.SingleValue)]
        public string BucketName { get; set; }

        [Option("-f|--file", "FileName", CommandOptionType.SingleValue)]
        public string FileName { get; set; }

        [Required]
        [Option("-p|--path", "UploadPath", CommandOptionType.SingleValue)]
        public string UploadPath { get; set; }

        protected override void Execute(CommandLineApplication _)
        {
            UploadPath = Path.GetFullPath(UploadPath);

            if (string.IsNullOrWhiteSpace(FileName))
            {
                FileName = Path.GetFileName(UploadPath);
            }

            var storageFile = _storageHelper.UploadStorageFile(BucketName, FileName, UploadPath);
            ConsoleHelper.RenderStorageFile(storageFile);
        }

        protected override bool HasValidOptions()
        {
            return !string.IsNullOrWhiteSpace(UploadPath) && File.Exists(UploadPath);
        }

        private static string GetVersion() => GetVersion(typeof(UploadCommand));
    }
}