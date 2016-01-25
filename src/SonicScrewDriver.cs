using Newtonsoft.Json;
using SonicScrewDriver.Project;
using System;
using System.Diagnostics;
using System.IO;

namespace SonicScrewDriver {

    public class SonicScrewDriver {

        public static void Main(string[] args) {
            var projectJson = File.ReadAllText("./project.json");
            ProjectConfiguration project = DeserialiseProject(projectJson);
            Command command = CreateCommand(project);
            EnsureDestinationDirectoryExists(command);
            BuildProject(command);
            CopyReferences(project, command);
            CopyResources(project, command);
        }

        public static ProjectConfiguration DeserialiseProject(string projectJson) {
            var configuration = JsonConvert.DeserializeObject<ProjectConfiguration>(projectJson);
            return configuration;
        }

        public static void BuildProject(Command command) {
            Console.WriteLine(command.GenerateArgumentList());
            var processInfo = new ProcessStartInfo(command.CommandName);
            processInfo.Arguments = command.GenerateArgumentList();
            processInfo.UseShellExecute = true;
            processInfo.WorkingDirectory = Directory.GetCurrentDirectory();
            Console.WriteLine(Directory.GetCurrentDirectory());
            Console.WriteLine("Build starting...");
            var process = new Process();
            process.EnableRaisingEvents = true;
            process.StartInfo = processInfo;
            process.Start();
            process.WaitForExit();
            Console.WriteLine("Build Completed.");
        }

        public static Command CreateCommand(ProjectConfiguration project) {
            return CommandBuilder.BuildCommand(project);
        }

        public static void EnsureDestinationDirectoryExists(Command command) {

            if(string.IsNullOrEmpty(command.DestinationDirectory)) {
                command.DestinationDirectory = "./build";
            }

            if(Directory.Exists(command.DestinationDirectory)) {
                // Directory already exists.
                return;
            }

            try {
                Directory.CreateDirectory(command.DestinationDirectory);
            }
            catch(Exception) {
                Console.WriteLine("Unable to create destination directory.");
                throw;
            }

        }

        public static void CopyReferences(ProjectConfiguration project, Command command) {

            if(project.References == null || project.References.Count == 0) {
                return;
            }

            var destinationDirectory = command.DestinationDirectory;

            foreach(var reference in project.References) {

                if(string.IsNullOrEmpty(reference.Path)) {
                    continue;
                }

                var path = reference.Path;
                var referenceName = reference.Name;
                var fileExtension = ".dll";

                File.Copy(string.Format("{0}{1}{2}", path, referenceName, fileExtension),
                          string.Format("{0}{1}{2}", destinationDirectory, referenceName, fileExtension),
                          true);
            }

        }

        public static void CopyResources(ProjectConfiguration project, Command command) {

            if(project.Resources == null || project.Resources.Count == 0) {
                return;
            }

            foreach(var resource in project.Resources) {
                var sourceDirectory = Path.GetDirectoryName(resource.Source);
                var destinationDirectory = Path.GetDirectoryName(resource.Destination);
                Console.WriteLine("Source Directory: {0}", sourceDirectory);
                Console.WriteLine("Destination Directory: {0}", destinationDirectory);

                if(!Directory.Exists(string.Format("{0}{1}", command.DestinationDirectory, destinationDirectory))) {

                    try {
                        Directory.CreateDirectory(string.Format("{0}{1}", command.DestinationDirectory, destinationDirectory));
                    }
                    catch(Exception) {
                        Console.WriteLine("Unable to create destination directory.");
                        throw;
                    }

                }

                File.Copy(string.Format("{0}{1}", command.SourceDirectory, resource.Source),
                          string.Format("{0}{1}", command.DestinationDirectory, resource.Destination),
                          true);

            }
        }

    }

}
