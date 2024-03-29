namespace SonicScrewDriver.Project {

    public class Command {
        string commandName = "mcs";
        string debugFlag = "-debug";
        string buildTarget = "exe";
        string sourceDirectory = "./src/";
        string destinationDirectory = "./build";
        string references = "";

        public string CommandName {

            get {
                return commandName;
            }

            set {
                commandName = value;
            }

        }

        public string DebugFlag {

            get {
                return debugFlag;
            }

            set {
                debugFlag = value;
            }

        }

        public string OutputFilename {
            get; set;
        }

        public string SourceFiles  {
            get; set;
        }

        public string BuildTarget {
            get {
                return buildTarget;
            }

            set{
                buildTarget = value;
            }
        }

        public string References {
            get {
                return references;
            }

            set {
                references = value;
            }
        }

        public string SourceDirectory {

            get{
                return sourceDirectory;
            }

            set{
                sourceDirectory = value;
            }

        }

        public string DestinationDirectory  {

            get{
                return destinationDirectory ;
            }

            set{
                destinationDirectory  = value;
            }

        }

        public string LibraryPath { get; set; }

        public string PackageList { get; set; }

        public string WarningLevel { get; set; }

        public string WarningsAsErrors { get; set; }

        public string GenerateArgumentList() {
            var argumentArray = new string[] {
                SourceFiles, DebugFlag, OutputFilename + GetFileSuffix(),
                BuildTarget, References, LibraryPath,
                WarningLevel, WarningsAsErrors
            };

            return string.Join(" ", argumentArray);
        }

        public string GetFileSuffix() {
            string suffix;

            switch(BuildTarget) {
                case "-target:exe":
                    suffix = ".exe";
                    break;
                case "-target:library":
                    suffix =  ".dll";
                    break;
                case "-target:module":
                    suffix = ".netmodule";
                    break;
                case "target:winexe":
                    suffix = ".exe";
                    break;
                default:
                    suffix = ".exe";
                    break;
            }

            return suffix;
        }
    }
}
