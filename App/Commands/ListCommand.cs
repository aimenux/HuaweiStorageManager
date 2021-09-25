using System.ComponentModel.DataAnnotations;
using Lib.Helpers;
using McMaster.Extensions.CommandLineUtils;

namespace App.Commands
{
    [Command(Name = "List", FullName = "List buckets/files", Description = "List buckets/files.")]
    [VersionOptionFromMember(MemberName = nameof(GetVersion))]
    [HelpOption]
    public class ListCommand : AbstractCommand
    {
        private readonly IStorageHelper _storageHelper;

        public ListCommand(IStorageHelper storageHelper, IConsoleHelper consoleHelper) : base(consoleHelper)
        {
            _storageHelper = storageHelper;
        }

        [Option("-b|--bucket", "Bucket Name", CommandOptionType.SingleValue)]
        public string BucketName { get; set; }

        [Range(1, 1000)]
        [Option("-m|--max", "MaxItems", CommandOptionType.SingleValue)]
        public int MaxItems { get; set; } = 30;

        protected override void Execute(CommandLineApplication _)
        {
            var storageFiles = _storageHelper.ListStorageFiles(BucketName, MaxItems);
            ConsoleHelper.RenderStorageFiles(storageFiles);
        }

        protected override bool HasValidOptions()
        {
            return !string.IsNullOrWhiteSpace(BucketName);
        }

        private static string GetVersion() => GetVersion(typeof(ListCommand));
    }
}