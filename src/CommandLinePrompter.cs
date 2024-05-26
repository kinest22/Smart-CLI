using SmartCLI.Commands;

namespace SmartCLI
{
    internal static class CommandLinePrompter
    {
        public static string GetNextCommandPrompt(string input, CommandSpace space, out Command? cmd)        
        {
            cmd = CliUnitSearchEngine.SearchForward(input, space.Commands);
            return GetPrompt(cmd, input);
        }

        public static string GetPreviousCommandPrompt(string input, CommandSpace space, out Command? cmd)
        {
            cmd = CliUnitSearchEngine.SearchBackward(input, space.Commands);
            return GetPrompt(cmd, input);
        }

        public static string GetNextSubcommandPrompt(string input, Command parentCmd, out Command? cmd)
        {
            cmd = CliUnitSearchEngine.SearchForward(input, parentCmd.Subcommands);
            return GetPrompt(cmd, input);
        }

        public static string GetPreviousSubcommandPrompt(string input, Command parentCmd, out Command? cmd)
        {
            cmd = CliUnitSearchEngine.SearchBackward(input, parentCmd.Subcommands);
            return GetPrompt(cmd, input);
        }

        public static string GetNextOptionPrompt(string input, Command cmd)
        {
            var opt = CliUnitSearchEngine.SearchForward(input, cmd.Options);
            return GetPrompt(opt, input);
        }

        public static string GetPreviousOptionPrompt(string input, Command cmd)
        {
            var opt = CliUnitSearchEngine.SearchForward(input, cmd.Options);
            return GetPrompt(opt, input);
        }

        public static string GetHint(string[] input, Command cmd)
        {
            return string.Empty;
        }

        private static string GetPrompt(ISearchableUnit? unit, string input)
        {
            if (unit != null)
                return unit.Name[input.Length..];
            return string.Empty;
        }

    }


    internal enum ParsingUnit
    {
        None,
        Command,
        Option,
        Argument
    }
}
