using Newtonsoft.Json;
using SonicScrewDriver.Project;
using System;
using System.IO;

namespace SonicScrewDriver {

    public class SonicScrewDriver {

        public static void Main(string[] args) {
            var projectJson = File.ReadAllText("./project.json");
            ProjectConfiguration project = DeserialiseProject(projectJson);
            Console.WriteLine(project.Name);
            Console.WriteLine(project.Description);

            foreach(var reference in project.References){
                Console.WriteLine("Name: {0}, Path: {1}", reference.Name, reference.Path);
            }

            foreach(var sourceFile in project.SourceFiles) {
                Console.WriteLine(sourceFile);
            }
        }

        public static ProjectConfiguration DeserialiseProject(string projectJson) {
            var configuration = JsonConvert.DeserializeObject<ProjectConfiguration>(projectJson);
            return configuration;
        }

    }

}
