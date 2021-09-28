using System;
using System.ComponentModel.DataAnnotations;
using Lib.Helpers;
using McMaster.Extensions.CommandLineUtils;

namespace App.Commands
{
    [Command(Name = "Copy", FullName = "Copy a bucket file", Description = "Copy a bucket file.")]
    [VersionOptionFromMember(MemberName = nameof(GetVersion))]
    [HelpOption]
    public class CopyCommand : AbstractCommand
    {
        private readonly IStorageHelper _storageHelper;

        public CopyCommand(IStorageHelper storageHelper, IConsoleHelper consoleHelper) : base(consoleHelper)
        {
            _storageHelper = storageHelper;
        }

        [Required]
        [Option("-sb|--source-bucket", "Source BucketName", CommandOptionType.SingleValue)]
        public string SourceBucketName { get; set; }

        [Required]
        [Option("-sf|--source-file", "Source FileName", CommandOptionType.SingleValue)]
        public string SourceFileName { get; set; }

        [Option("-tb|--target-bucket", "Target BucketName", CommandOptionType.SingleValue)]
        public string TargetBucketName { get; set; }

        [Required]
        [Option("-tf|--target-file", "Target FileName", CommandOptionType.SingleValue)]
        public string TargetFileName { get; set; }

        protected override void Execute(CommandLineApplication _)
        {
            var storageFile = _storageHelper.CopyStorageFile(SourceBucketName, SourceFileName, TargetBucketName, TargetFileName);
            ConsoleHelper.RenderStorageFile(storageFile);
        }

        protected override void Initialize()
        {
            if (string.IsNullOrWhiteSpace(TargetBucketName))
            {
                TargetBucketName = SourceBucketName;
            }
        }

        protected override bool HasValidOptions()
        {
            if (string.IsNullOrWhiteSpace(SourceBucketName) || string.IsNullOrWhiteSpace(SourceFileName))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(TargetBucketName) || string.IsNullOrWhiteSpace(TargetFileName))
            {
                return false;
            }

            if (string.Equals(SourceBucketName, TargetBucketName, StringComparison.OrdinalIgnoreCase))
            {
                return !string.Equals(SourceFileName, TargetFileName, StringComparison.OrdinalIgnoreCase);
            }

            return true;
        }

        private static string GetVersion() => GetVersion(typeof(CopyCommand));
    }
}