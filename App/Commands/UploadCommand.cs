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

        [Option("--headers", "HeadersFile", CommandOptionType.SingleValue)]
        public string HeadersFile { get; set; }

        protected override void Execute(CommandLineApplication _)
        {
            var storageFile = _storageHelper.UploadStorageFile(BucketName, FileName, UploadPath, HeadersFile);
            ConsoleHelper.RenderStorageFile(storageFile);
        }

        protected override void Initialize()
        {
            UploadPath = Path.GetFullPath(UploadPath);

            if (string.IsNullOrWhiteSpace(FileName))
            {
                FileName = Path.GetFileName(UploadPath);
            }
        }

        protected override bool HasValidOptions()
        {
            if (string.IsNullOrWhiteSpace(UploadPath) || !File.Exists(UploadPath))
            {
                return false;
            }

            if (!string.IsNullOrWhiteSpace(HeadersFile))
            {
                return File.Exists(HeadersFile);
            }

            return true;
        }

        private static string GetVersion() => GetVersion(typeof(UploadCommand));
    }
}