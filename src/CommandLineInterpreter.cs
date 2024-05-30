using SmartCLI.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartCLI
{
    internal class CommandLineInterpreter
    {
#pragma warning disable IDE1006 // Naming Styles
        private const ConsoleKey BKSP = ConsoleKey.Backspace;
        private const ConsoleKey TAB = ConsoleKey.Tab;
        private const ConsoleKey ENTR = ConsoleKey.Enter;
        private const ConsoleKey ESC = ConsoleKey.Escape;
        private const ConsoleKey UARR = ConsoleKey.UpArrow;
        private const ConsoleKey DARR = ConsoleKey.DownArrow;
        private const ConsoleKey DASH1 = ConsoleKey.Subtract;
        private const ConsoleKey DASH2 = ConsoleKey.OemMinus;
        private const ConsoleModifiers ALT = ConsoleModifiers.Alt;
        private const ConsoleModifiers SHFT = ConsoleModifiers.Shift;
        private const ConsoleModifiers CTRL = ConsoleModifiers.Control;
#pragma warning restore IDE1006 // Naming Styles


        private readonly IEnumerable<CommandSpace> _cmdspaces;
        private readonly StringBuilder _buffer;

        private uint _pos;
        private CommandSpace? _spaceGuess;
        private CommandSpace? _spaceDefined;
        private Command? _cmdGuess;
        private Command? _cmdDefined;
        private CommandParameter? _optGuess;



        internal CommandLineInterpreter(IEnumerable<CommandSpace> cmdspaces)
        {
            _cmdspaces = cmdspaces;
            _buffer = new StringBuilder();
        }





        /// <summary>
        /// 
        /// </summary>
        internal event Action<string>? PromptDefined;






        internal void InterpretInput(ConsoleKeyInfo cki)
        {
            var key = cki.Key;
            var mod = cki.Modifiers;
            var ch = cki.KeyChar;

            switch (key)
            {
                case ENTR:
                    goto case ESC;
                case ESC:
                    break;
            }



            switch(key, mod)
            {
                case (BKSP, _):
                    break;

                case (TAB, _):
                    break;

                case (ENTR, SHFT):
                    break;

                case (ENTR, _):
                    break;

                case (ESC, _):
                    break;

                case (UARR, _):
                    break;

                case (DARR, _):
                    break;

                case (DASH1, _):
                    break;

                case (DASH2, _):
                    break;

                default:
                    if ((mod & (ALT | CTRL)) > 0 || ch == '\0') 
                        break;
                    else 
                        AddToBuffer(ch);
                    break;
            }
        }





        private void AddToBuffer(char ch)
        {
            _buffer.Append(ch);
            _pos++;
        }

        private void DefineUnit()
        {

        }







        private string GetNextCommandPrompt(string input, CommandSpace space, out Command? cmd)
        {
            cmd = CliUnitSearchEngine.SearchForward(input, space.Commands);
            return GetPrompt(cmd, input);
        }

        private string GetPreviousCommandPrompt(string input, CommandSpace space, out Command? cmd)
        {
            cmd = CliUnitSearchEngine.SearchBackward(input, space.Commands);
            return GetPrompt(cmd, input);
        }

        private string GetNextSubcommandPrompt(string input, Command parentCmd, out Command? cmd)
        {
            cmd = CliUnitSearchEngine.SearchForward(input, parentCmd.Subcommands);
            return GetPrompt(cmd, input);
        }

        private string GetPreviousSubcommandPrompt(string input, Command parentCmd, out Command? cmd)
        {
            cmd = CliUnitSearchEngine.SearchBackward(input, parentCmd.Subcommands);
            return GetPrompt(cmd, input);
        }

        private string GetNextOptionPrompt(string input, Command cmd)
        {
            var opt = CliUnitSearchEngine.SearchForward(input, cmd.Options);
            return GetPrompt(opt, input);
        }

        private string GetPreviousOptionPrompt(string input, Command cmd)
        {
            var opt = CliUnitSearchEngine.SearchForward(input, cmd.Options);
            return GetPrompt(opt, input);
        }

        private string GetHint(string[] input, Command cmd)
        {
            return string.Empty;
        }

        private string GetPrompt(ISearchableUnit? unit, string input)
        {
            if (unit != null)
                return unit.Name[input.Length..];
            return string.Empty;
        }
    }
}
