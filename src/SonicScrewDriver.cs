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
            var buildStatistics = BuildProject(command);
            TimeSpan timeSpan = buildStatistics.ElapsedTime;
            string elapsedTime;

            if(buildStatistics.ExitCode != 0) {
                Console.WriteLine("Build Failed.");
                elapsedTime = string.Format("{0:00}:{1:00}:{2:00}.{3:000}", timeSpan.Hours, timeSpan.Minutes,
                timeSpan.Seconds, timeSpan.Milliseconds / 100);
                Console.WriteLine("Build took {0}", elapsedTime);
                return;
            }

            Console.WriteLine("Build completed successfully.");
            elapsedTime = string.Format("{0:00}:{1:00}:{2:00}.{3:000}", timeSpan.Hours, timeSpan.Minutes,
                timeSpan.Seconds, timeSpan.Milliseconds / 100);
            Console.WriteLine("Build took {0}", elapsedTime);
            CopyReferences(project, command);
            CopyResources(project, command);
        }

        public static ProjectConfiguration DeserialiseProject(string projectJson) {
            var configuration = JsonConvert.DeserializeObject<ProjectConfiguration>(projectJson);
            return configuration;
        }

        public static BuildStatistics BuildProject(Command command) {
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
            var stopWatch = Stopwatch.StartNew();
            process.Start();
            process.WaitForExit();
            stopWatch.Stop();

            return new BuildStatistics {
                ExitCode = process.ExitCode,
                ElapsedTime = stopWatch.Elapsed
            };
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
