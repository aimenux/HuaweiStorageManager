using System.ComponentModel.DataAnnotations;
using Lib.Helpers;
using McMaster.Extensions.CommandLineUtils;

namespace App.Commands
{
    [Command(Name = "Delete", FullName = "Delete a bucket file", Description = "Delete a bucket file.")]
    [VersionOptionFromMember(MemberName = nameof(GetVersion))]
    [HelpOption]
    public class DeleteCommand : AbstractCommand
    {
        private readonly IStorageHelper _storageHelper;

        public DeleteCommand(IStorageHelper storageHelper, IConsoleHelper consoleHelper) : base(consoleHelper)
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
            var storageFile = _storageHelper.DeleteStorageFile(BucketName, FileName);
            ConsoleHelper.RenderStorageFile(storageFile);
        }

        protected override bool HasValidOptions()
        {
            return !string.IsNullOrWhiteSpace(BucketName)
                   && !string.IsNullOrWhiteSpace(FileName);
        }

        private static string GetVersion() => GetVersion(typeof(DeleteCommand));
    }
}