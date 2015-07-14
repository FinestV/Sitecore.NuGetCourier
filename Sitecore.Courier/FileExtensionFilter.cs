using System.Collections.Generic;
using System.Linq;
using Sitecore.Update.Commands;
using Sitecore.Update.Interfaces;

namespace Sitecore.Courier
{
    public class FileExtensionFilter : ICommandFilter
    {
        public List<string> Extensions { get; private set; }

        public FileExtensionFilter()
        {
            Extensions = new List<string>();
        }
        public ICommandFilter Clone()
        {
            var fileExtFilter = new FileExtensionFilter();
            foreach (var str in Extensions)
            {
                if (str != null)
                    fileExtFilter.Extensions.Add(str.Clone() as string);
                else
                    fileExtFilter.Extensions.Add(null);
            }
            return fileExtFilter;
        }
        public ICommand FilterCommand(ICommand command)
        {
            if (command is DeleteFolderCommand)
                return command;
            if (!(command is AddFileCommand || command is ChangeFileCommand || command is DeleteFileCommand))
                return null;
            if (Extensions.Any(ext => !string.IsNullOrEmpty(ext) && command.EntityPath.EndsWith(ext)))
            {
                return null;
            }
            return command;
        }
    }
}
