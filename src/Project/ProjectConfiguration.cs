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

        public string DestinationDirectory {
            get; set;
        }

        public List<Reference> References {
            get; set;
        }

        public string SourceDirectory {
            get; set;
        }

        public List<string> SourceFiles {
            get; set;
        }
    }

}
