using System.Collections.Generic;

namespace SonicScrewDriver.Project {

    public class ProjectConfiguration {

        public string Name {
            get; set;
        }

        public string Version {
            get; set;
        }

        public string Description {
            get; set;
        }

        public List<Reference> References {
            get; set;
        }

        public List<string> SourceFiles {
            get; set;
        }

        public List<Resource> Resources {
            get; set;
        }

        public string BuildTarget {
            get; set;
        }

        public string OutputFilename {
            get; set;
        }

        public string SourceDirectory {
            get; set;
        }

        public string DestinationDirectory {
            get; set;
        }

        public List<string> LibraryPath {
            get; set;
        }

        public List<string> PackageList {
            get; set;
        }

        public int WarningLevel {
            get; set;
        }

        public bool WarningsAsErrors {
            get; set;
        }
    }

}
