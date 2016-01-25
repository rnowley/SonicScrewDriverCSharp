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
            BuildProject(command);
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

    }

}
