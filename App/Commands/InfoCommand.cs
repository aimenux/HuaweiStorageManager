using System.ComponentModel.DataAnnotations;
using Lib.Helpers;
using McMaster.Extensions.CommandLineUtils;

namespace App.Commands
{
    [Command(Name = "Info", FullName = "Info about a bucket file", Description = "Info about a bucket file.")]
    [VersionOptionFromMember(MemberName = nameof(GetVersion))]
    [HelpOption]
    public class InfoCommand : AbstractCommand
    {
        private readonly IStorageHelper _storageHelper;

        public InfoCommand(IStorageHelper storageHelper, IConsoleHelper consoleHelper) : base(consoleHelper)
        {
            _storageHelper = storageHelper;
        }

        [Required]
        [Option("-b|--bucket", "Bucket Name", CommandOptionType.SingleValue)]
        public string BucketName { get; set; }

        [Required]
        [Option("-f|--file", "FileName", CommandOptionType.SingleValue)]
        public string FileName { get; set; }

        protected override void Execute(CommandLineApplication _)
        {
            var storageFile = _storageHelper.GetStorageFileInfo(BucketName, FileName);
            ConsoleHelper.RenderStorageFile(storageFile);
        }

        protected override bool HasValidOptions()
        {
            return !string.IsNullOrWhiteSpace(BucketName)
                   && !string.IsNullOrWhiteSpace(FileName);
        }

        private static string GetVersion() => GetVersion(typeof(InfoCommand));
    }
}