using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Sitecore.NuGetCourier.Runner
{
    public static class Extensions
    {
        //If extensionless path add * to the end to work around bug in nuget pack with extensionless files
        public static string EnsureExtension(this string path)
        {
            if (Path.HasExtension(path))
                return path;

            return path + "*";

        }
        public static string GetTarget(this string filePath)
        {
            return filePath.Substring(0, filePath.LastIndexOf("\\", StringComparison.Ordinal));
        }

        public static string CreateReleaseNotesSection(this List<string> items, string heading)
        {
            if (items.Count > 0)
            {
                var sb = new StringBuilder();
                sb.AppendLine("<p>");
                sb.AppendLine(string.Format("<b>{0} ({1})</b><br/>", heading, items.Count));
                items.ForEach(item => sb.AppendLine(item + "</br>"));
                sb.AppendLine("<p/>");
                return sb.ToString().Replace("_", "&#95;"); //Fix problem with displaying underscores on std value items
            }
            return "";
        }
    }
}
