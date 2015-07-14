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
            HelpText = "Id of generated nuspec file")]
        public string NuspecId { get; set; }


        [Option('c', "config", Required = false,
            HelpText = "Include all config files to transform at deploy time")]
        public bool IncludeConfigs { get; set; }

        [Option('p', "pack", Required = false,
            HelpText = "Pack the generated nuspec file to create a nupkg with supplied version number")]
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
