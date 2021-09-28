using System;
using System.Collections.Generic;
using System.IO;
using Lib.Extensions;
using Lib.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OBS;
using Spectre.Console;

namespace Lib.Helpers
{
    public class ConsoleHelper : IConsoleHelper
    {
        public void RenderTitle(string text)
        {
            AnsiConsole.WriteLine();
            AnsiConsole.Render(new FigletText(text).LeftAligned());
            AnsiConsole.WriteLine();
        }

        public void RenderSettingsFile(string filepath)
        {
            var name = Path.GetFileName(filepath);
            var json = File.ReadAllText(filepath);
            var formattedJson = JToken.Parse(json).ToString(Formatting.Indented);
            var header = new Rule($"[yellow]({name})[/]");
            header.Centered();
            var footer = new Rule($"[yellow]({filepath})[/]");
            footer.Centered();

            AnsiConsole.WriteLine();
            AnsiConsole.Render(header);
            AnsiConsole.WriteLine(formattedJson);
            AnsiConsole.Render(footer);
            AnsiConsole.WriteLine();
        }

        public void RenderStorageFiles(ICollection<StorageFile> storageFiles)
        {
            var table = new Table()
                .BorderColor(Color.White)
                .Border(TableBorder.Square)
                .Title($"[yellow]{storageFiles.Count} file(s)[/]")
                .AddColumn(new TableColumn("[u]Name[/]").Centered())
                .AddColumn(new TableColumn("[u]Size[/]").Centered())
                .AddColumn(new TableColumn("[u]ModificationDate[/]").Centered());

            foreach (var storageObject in storageFiles)
            {
                table.AddRow(
                    storageObject.Name.GetValueOrEmpty(),
                    storageObject.Size.GetValueOrEmpty(),
                    storageObject.ModificationDate.GetValueOrEmpty());
            }

            AnsiConsole.WriteLine();
            AnsiConsole.Render(table);
            AnsiConsole.WriteLine();
        }

        public void RenderStorageFile(StorageFile storageFile)
        {
            var table1 = new Table()
                .BorderColor(Color.White)
                .Border(TableBorder.Rounded)
                .AddColumn(new TableColumn("[u][dim]Key[/][/]").Centered())
                .AddColumn(new TableColumn("[u][dim]Value[/][/]").Centered());

            table1.AddRow(nameof(storageFile.Name), storageFile.Name.GetValueOrEmpty());
            table1.AddRow(nameof(storageFile.Size), storageFile.Size.GetValueOrEmpty());
            table1.AddRow(nameof(storageFile.ModificationDate), storageFile.ModificationDate.GetValueOrEmpty());

            var table2 = new Table()
                .BorderColor(Color.White)
                .Border(TableBorder.Rounded)
                .AddColumn(new TableColumn("[u][dim]Key[/][/]").Centered())
                .AddColumn(new TableColumn("[u][dim]Value[/][/]").Centered());

            foreach (var (key, value) in storageFile.Headers)
            {
                table2.AddRow(key.GetValueOrEmpty().ToUpper(), value.GetValueOrEmpty());
            }

            var table = new Table()
                .BorderColor(Color.White)
                .Border(TableBorder.Square)
                .Title("[yellow]Info[/]")
                .AddColumn(new TableColumn("[u][dim]General[/][/]").Centered())
                .AddColumn(new TableColumn("[u][dim]Headers[/][/]").Centered());

            table.AddRow(table1, table2);

            AnsiConsole.WriteLine();
            AnsiConsole.Render(table);
            AnsiConsole.WriteLine();
        }

        public void RenderStorageFile(UploadStorageFile storageFile)
        {
            var uploadPathMarkup = $"[green][link={storageFile.UploadPath}]{storageFile.UploadPath}[/][/]";

            var table = new Table()
                .BorderColor(Color.White)
                .Border(TableBorder.Square)
                .Title("[yellow]1 file(s) uploaded[/]")
                .AddColumn(new TableColumn("[u]BucketName[/]").Centered())
                .AddColumn(new TableColumn("[u]FileName[/]").Centered())
                .AddColumn(new TableColumn("[u]UploadPath[/]").Centered());

            table.AddRow(storageFile.BucketName, storageFile.FileName, uploadPathMarkup);

            AnsiConsole.WriteLine();
            AnsiConsole.Render(table);
            AnsiConsole.WriteLine();
        }

        public void RenderStorageFile(DownloadStorageFile storageFile)
        {
            var downloadPathMarkup = $"[green][link={storageFile.DownloadPath}]{storageFile.DownloadPath}[/][/]";

            var table = new Table()
                .BorderColor(Color.White)
                .Border(TableBorder.Square)
                .Title("[yellow]1 file(s) downloaded[/]")
                .AddColumn(new TableColumn("[u]BucketName[/]").Centered())
                .AddColumn(new TableColumn("[u]FileName[/]").Centered())
                .AddColumn(new TableColumn("[u]DownloadPath[/]").Centered());

            table.AddRow(storageFile.BucketName, storageFile.FileName, downloadPathMarkup);

            AnsiConsole.WriteLine();
            AnsiConsole.Render(table);
            AnsiConsole.WriteLine();
        }

        public void RenderStorageFile(CopyStorageFile storageFile)
        {
            var table = new Table()
                .BorderColor(Color.White)
                .Border(TableBorder.Square)
                .Title("[yellow]1 file(s) copied[/]")
                .AddColumn(new TableColumn("[u]SourceBucketName[/]").Centered())
                .AddColumn(new TableColumn("[u]SourceFileName[/]").Centered())
                .AddColumn(new TableColumn("[u]TargetBucketName[/]").Centered())
                .AddColumn(new TableColumn("[u]TargetFileName[/]").Centered());

            table.AddRow(storageFile.SourceBucketName, storageFile.SourceFileName, storageFile.TargetBucketName, storageFile.TargetFileName);

            AnsiConsole.WriteLine();
            AnsiConsole.Render(table);
            AnsiConsole.WriteLine();
        }

        public void RenderStorageFile(DeleteStorageFile storageFile)
        {
            var table = new Table()
                .BorderColor(Color.White)
                .Border(TableBorder.Square)
                .Title("[yellow]1 file(s) deleted[/]")
                .AddColumn(new TableColumn("[u]BucketName[/]").Centered())
                .AddColumn(new TableColumn("[u]FileName[/]").Centered());

            table.AddRow(storageFile.BucketName, storageFile.FileName);

            AnsiConsole.WriteLine();
            AnsiConsole.Render(table);
            AnsiConsole.WriteLine();
        }

        public void RenderException(Exception exception)
        {
            const ExceptionFormats formats = ExceptionFormats.ShortenTypes
                                             | ExceptionFormats.ShortenPaths
                                             | ExceptionFormats.ShortenMethods;

            AnsiConsole.WriteLine();
            AnsiConsole.WriteException(exception, formats);
            AnsiConsole.WriteLine();
        }

        public void RenderException(ObsException exception)
        {
            RenderException(StorageException.InternalStorageFailure(exception));
            AnsiConsole.WriteLine();
        }
    }
}