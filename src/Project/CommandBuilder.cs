using System.Collections.Generic;

namespace SonicScrewDriver.Project {

    public static class CommandBuilder {

        public static Command BuildCommand(ProjectConfiguration configuration) {
            var command = new Command();
            command.SourceDirectory = string.IsNullOrEmpty(configuration.SourceDirectory)
                ? command.SourceDirectory : configuration.SourceDirectory;
            command.DestinationDirectory = string.IsNullOrEmpty(configuration.DestinationDirectory)
                ? command.DestinationDirectory : configuration.DestinationDirectory;
            command.OutputFilename = string.IsNullOrEmpty(configuration.OutputFilename)
                ? string.Format("-out:{0}{1}", command.DestinationDirectory, configuration.OutputFilename)
                : "";
            command.SourceFiles = ExtractSourceFileList(configuration, command.SourceDirectory);
            command.BuildTarget = ExtractBuildTarget(configuration);
            command.References = ExtractReferences(configuration);

            return command;
        }

        public static string ExtractSourceFileList(ProjectConfiguration configuration, string sourceDirectory) {
            var fileList = new List<string>();

            foreach(var sourceFile in configuration.SourceFiles) {
                fileList.Add(sourceDirectory + sourceFile);
            }

            return string.Join(" ", fileList);
        }

        public static string ExtractBuildTarget(ProjectConfiguration configuration) {

            switch(configuration.BuildTarget) {
                case "exe":
                case "library":
                case "module":
                case "winexe":
                    return string.Format("-target:{0}", configuration.BuildTarget);
                default:
                    return "-target:exe";
            }

        }

        public static string ExtractReferences(ProjectConfiguration configuration) {

            if(configuration.References.Count == 0) {
                return string.Empty;
            }

            var referenceNames = new List<string>();

            foreach(var reference in configuration.References) {
                referenceNames.Add(reference.Name);
            }

            var referenceList = string.Join(",", referenceNames);

            return string.Format("-r:{0}", referenceList);
        }

    }
}
