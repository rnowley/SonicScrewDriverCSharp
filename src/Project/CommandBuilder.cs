namespace SonicScrewDriver.Project {

    public static class CommandBuilder {

        public static Command BuildCommand(ProjectConfiguration configuration) {
            var command = new Command();
            command.DestinationDirectory = string.IsNullOrEmpty(configuration.DestinationDirectory)
                ? command.DestinationDirectory : configuration.DestinationDirectory;
            command.SourceDirectory = string.IsNullOrEmpty(configuration.SourceDirectory)
                ? command.SourceDirectory : configuration.SourceDirectory;

            return command;
        }

    }
}
