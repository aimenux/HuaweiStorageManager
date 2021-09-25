using System.IO;
using System.Reflection;
using Lib.Helpers;
using McMaster.Extensions.CommandLineUtils;

namespace App.Commands
{
    [Command(Name = "HuaweiStorage", FullName = "Manage HuaweiStorage", Description = "Manage HuaweiStorage.")]
    [Subcommand(typeof(ListCommand), typeof(InfoCommand), typeof(UploadCommand), typeof(DownloadCommand))]
    [VersionOptionFromMember(MemberName = nameof(GetVersion))]
    public class MainCommand : AbstractCommand
    {
        public MainCommand(IConsoleHelper consoleHelper) : base(consoleHelper)
        {
        }

        [Option("-s|--settings", "Show settings information.", CommandOptionType.NoValue)]
        public bool ShowSettings { get; set; }

        protected override void Execute(CommandLineApplication app)
        {
            if (ShowSettings)
            {
                var filepath = GetSettingFilePath();
                ConsoleHelper.RenderSettingsFile(filepath);
                return;
            }

            const string title = "HuaweiStorage";
            ConsoleHelper.RenderTitle(title);
            app.ShowHelp();
        }

        protected static string GetVersion() => GetVersion(typeof(MainCommand));

        private static string GetSettingFilePath() => Path.GetFullPath(Path.Combine(GetDirectoryPath(), @"appsettings.json"));

        private static string GetDirectoryPath()
        {
            try
            {
                return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            }
            catch
            {
                return Directory.GetCurrentDirectory();
            }
        }
    }
}
