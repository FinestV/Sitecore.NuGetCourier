using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using CommandLine;
using NuGet;
using Sitecore.Courier;
using Sitecore.Update.Commands;
using Sitecore.Update.Interfaces;

namespace Sitecore.NuGetCourier.Runner
{
    class Program
    {
        private const string DeployFile = "Deploy.ps1";
        private const string DeletesFile = "Deletes.Txt";
        private const string NuGetExe = "NuGet.exe";
        private const string NuspecExt = ".nuspec";
        private const string NupkgExt = ".nupkg";
        static readonly CmdLineOptions Options = new CmdLineOptions();
        static void Main(string[] args)
        {
            if (Parser.Default.ParseArguments(args, Options))
            {
                Console.WriteLine("SourceDir Dir: {0}", Options.SourceDir);
                Console.WriteLine("TargetDir Dir: {0}", Options.TargetDir);
                Console.WriteLine("Nuspec ID: {0}", Options.NuspecId);
                if (Options.IncludeConfigs)
                {
                    Console.WriteLine("Include all config files for transformation at deployment time = true");
                }
                if (!string.IsNullOrEmpty(Options.Pack))
                {
                    Console.WriteLine("Packing NuGet Package with version: {0}", Options.Pack);
                }
                //Generate the diff commands 
                var diffCommands = DiffGenerator.GetDiffCommands(Options.SourceDir, Options.TargetDir);
                GetNuspecItems(diffCommands);
            }
            else
            {
                Console.WriteLine(Options.GetUsage());
            }
        }

        public static void GetNuspecItems(List<ICommand> commands)
        {
            var addedFiles = new List<string>();
            var changedFiles = new List<string>();
            var deletedFiles = new List<string>();

            var deletes = new List<string>();
            var manifestFiles = new List<ManifestFile>();

            foreach (var command in commands)
            {
                if (command is AddFileCommand)
                {
                    addedFiles.Add(command.EntityPath);
                    manifestFiles.Add(new ManifestFile { Source = command.EntityPath.EnsureExtension(), Target = command.EntityPath.GetTarget() });
                }
                else if (command is ChangeFileCommand)
                {
                    changedFiles.Add(command.EntityPath);
                    manifestFiles.Add(new ManifestFile { Source = command.EntityPath.EnsureExtension(), Target = command.EntityPath.GetTarget() });
                }
                else if (command is DeleteFileCommand)
                {
                    deletedFiles.Add(command.EntityPath);
                    deletes.Add(command.EntityPath);
                }
                else if (command is DeleteFolderCommand)
                {
                    deletes.Add(command.EntityPath);
                }
            }
            //Add all config files so we can transform at deploy time not build time
            if (Options.IncludeConfigs)
            {
                manifestFiles.Add(new ManifestFile { Source = "**\\*.config", Target = "\\" });
            }
            var releaseNotes = CreateReleaseNotes(addedFiles, changedFiles, deletedFiles);
            var manifestFilePath = CreateManifest(manifestFiles, releaseNotes);
            CreateDeletesFile(deletes);
            CopyInstallFile();
            
            if (!string.IsNullOrEmpty(Options.Pack))
            {
                PackNuspec(manifestFilePath);
            }
        }
        public static string CreateReleaseNotes(List<string> addedFiles, List<string> changedFiles, List<string> deletedFiles)
        {
            return addedFiles.CreateReleaseNotesSection("Added Files")
                   + changedFiles.CreateReleaseNotesSection("Changed Files")
                   + deletedFiles.CreateReleaseNotesSection("Deleted Files");
        }
        public static string CreateManifest(List<ManifestFile> manifestFiles, string releaseNotes)
        {
            var manifest = new Manifest
            {
                Metadata = new ManifestMetadata
                {
                    Id = Options.NuspecId,
                    Version = "0.0.0", //overloaded by nuget pack cmd line parameters
                    Authors = "Sitecore NuGetPack Runner",
                    Description = string.Format("Sitecore NuGet Package, built on {0}", DateTime.Now.ToString("HH:mm dd/MM/yyyy")),
                    ReleaseNotes = releaseNotes
                },
                Files = manifestFiles
            };

            var manifestFilePath = Path.Combine(Options.TargetDir, Options.NuspecId + NuspecExt);
            using (var fsManifest = new FileStream(manifestFilePath, FileMode.Create))
            {
                manifest.Save(fsManifest, true);
            }
            return manifestFilePath;
        }
        public static void CreateDeletesFile(List<string> deletes)
        {
            if (deletes.Count <= 0) 
                return;

            var deletesFilePath = Path.Combine(Options.TargetDir, DeletesFile);
            using (var sw = new StreamWriter(deletesFilePath, false))
            {
                deletes.ForEach(d => sw.WriteLine(d));
            }
        }
        public static void CopyInstallFile()
        {
            File.Copy(DeployFile, string.Concat(Options.TargetDir, "\\", DeployFile), true);
        }
        public static void PackNuspec(string manifestFilePath)
        {
            var nugetExe = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, NuGetExe);
            var nugetPackProc = Process.Start(nugetExe, string.Format("pack {0} -Version {1} -OutputDirectory {2}", 
                manifestFilePath, Options.Pack, Environment.CurrentDirectory));
            if (nugetPackProc != null)
            {
                nugetPackProc.WaitForExit();
                var nugetPkgName = string.Concat(Options.NuspecId, ".", Options.Pack, NupkgExt);
                Console.WriteLine(
                    nugetPackProc.ExitCode == 0
                        ? "NuGet Package {0} created successfully"
                        : "Failed to create NuGet package {0}", nugetPkgName);
            }
        }
    }
}
