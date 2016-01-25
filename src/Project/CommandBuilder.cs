using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SonicScrewDriver.Project {

    public static class CommandBuilder {

        public static Command BuildCommand(ProjectConfiguration configuration) {
            var command = new Command();
            command.SourceDirectory = string.IsNullOrEmpty(configuration.SourceDirectory)
                ? command.SourceDirectory : configuration.SourceDirectory;
            command.DestinationDirectory = string.IsNullOrEmpty(configuration.DestinationDirectory)
                ? command.DestinationDirectory : configuration.DestinationDirectory;
            command.OutputFilename = string.IsNullOrEmpty(configuration.OutputFilename)
                ? ""
                : string.Format("-out:{0}{1}", command.DestinationDirectory, configuration.OutputFilename);
            command.SourceFiles = ExtractSourceFileList(configuration, command.SourceDirectory);
            command.BuildTarget = ExtractBuildTarget(configuration);
            command.References = ExtractReferences(configuration);
            command.LibraryPath = ExtractLibraryPath(configuration);
            command.PackageList = ExtractPackageList(configuration);
            command.WarningLevel = SetWarningLevel(configuration);
            command.WarningsAsErrors = TreatWarningsAsErrors(configuration);

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

        public static string ExtractLibraryPath(ProjectConfiguration configuration) {

            if(configuration.LibraryPath == null || configuration.LibraryPath.Count == 0) {
                return string.Empty;
            }

            var libraryPath = string.Join(",", configuration.LibraryPath);

            return string.Format("-lib:{0}", libraryPath);
        }

        public static string ExtractPackageList(ProjectConfiguration configuration) {

            if(configuration.PackageList == null || configuration.PackageList.Count == 0) {
                return string.Empty;
            }

            var packageList = string.Join(",", configuration.PackageList);

            return string.Format("-pkg:{0}", packageList);
        }

        public static string ExtractReferences(ProjectConfiguration configuration) {

            if(configuration.References == null || configuration.References.Count == 0) {
                return string.Empty;
            }

            var referenceList = new List<string>();
            foreach(var reference in configuration.References) {
                referenceList.Add(reference.Name);
            }

            var referenceString = string.Join(",", referenceList);

            return string.Format("-r:{0}", referenceString);
        }

        public static string SetWarningLevel(ProjectConfiguration configuration) {

            if(string.IsNullOrEmpty(configuration.WarningLevel)) {
                return string.Empty;
            }

            int warningLevel;
            bool isNumeric = int.TryParse(configuration.WarningLevel, out warningLevel);

            if(!isNumeric) {
                return string.Empty;
            }

            if(warningLevel >=0 && warningLevel <= 4) {
                return string.Format("-warn:{0}", warningLevel);
            }

            Console.WriteLine("Warning: Invalid value for warning level ({0}), using the default value for the compiler.",
                configuration.WarningLevel);
            return string.Empty;
        }

        public static string TreatWarningsAsErrors(ProjectConfiguration configuration) {

            if(string.IsNullOrEmpty(configuration.WarningsAsErrors)) {
                return string.Empty;
            }

            bool warningsAsErrors;
            bool isBoolean = bool.TryParse(configuration.WarningsAsErrors, out warningsAsErrors);

            if(!isBoolean) {
                Console.WriteLine("Warning: Invalid value for WarningsAsErrors, using the default value for the compiler.");
                return string.Empty;
            }

            return warningsAsErrors ? "-warnaserror+" : "-warnaserror-";
        }

    }
}
