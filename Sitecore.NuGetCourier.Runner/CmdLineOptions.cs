using CommandLine;
using CommandLine.Text;

namespace Sitecore.NuGetCourier.Runner
{
    class CmdLineOptions
    {
        [Option('s', "source", Required = true,
          HelpText = "Source directory")]
        public string SourceDir { get; set; }

        [Option('t', "target", Required = true,
          HelpText = "Target directory")]
        public string TargetDir { get; set; }

        [Option('n', "nuspec", Required = true,
            HelpText = "NuGet Manifest Id (#Id#.nuspec)")]
        public string NuspecId { get; set; }


        [Option('c', "config", Required = false,
            HelpText = "Include all config files (use if we wan't to transform at deploy not build time)")]
        public bool IncludeConfigs { get; set; }

        [Option('p', "pack", Required = false,
            HelpText = "Pack the generated NuGet Manifest file to create a NuGet Package with this version number")]
        public string Pack { get; set; }

        [ParserState]
        public IParserState LastParserState { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this,
              (current) => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }
}
