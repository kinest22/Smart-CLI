using SmartCLI.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartCLI
{
    internal class CliStateMachine
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
        private readonly CliUnitSearchEngine _searchEngine;
        private readonly List<ISearchableUnit> _unitsFound;
        private readonly StringBuilder _buffer;

        private State _state;

        private int _pos;
        private int _space_start_pos;
        private int _space_end_pos;
        private int _cmd_start_pos;
        private int _cmd_end_pos;


        private CommandSpace? _spaceGuess;
        private CommandSpace? _spaceDefined;
        private Command? _cmdGuess;
        private Command? _cmdDefined;
        private CommandParameter? _optGuess;

        private string _prompt;


        internal CliStateMachine(IEnumerable<CommandSpace> cmdspaces)
        {
            _cmdspaces = cmdspaces;
            _prompt = string.Empty;
            _buffer = new StringBuilder();
            _unitsFound = new List<ISearchableUnit>();
            _searchEngine = CliUnitSearchEngine.Create();
            RegisterCommandSpaces();
        }





        internal void ProcessInput(ConsoleKeyInfo cki)
        {
            var key = cki.Key;
            var mod = cki.Modifiers;
            var ch = cki.KeyChar;

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
                    {
                        break;
                    }
                    else
                    {
                        AddToBuffer(ch);
                        int len = RunSearch();
                        if (len > 0)
                        {

                        }
                    }
                        
                    break;
            }
        }





        private void AddToBuffer(char ch)
        {
            _buffer.Append(ch);
            _pos++;
        }

        // for symbol, backspace or whitespace
        private void UpdateState()
        {
            _unitsFound.Clear();
            if (_state == State.Initial)
            {
                string wildcard = _buffer.ToString(_space_start_pos, _pos - _space_start_pos);
                int len = _searchEngine.FindByWildcard(wildcard, _cmdspaces, in _unitsFound);
                if (len == 0)
                {
                    return;
                }                
                else if (len == 1 && _unitsFound[0].Name == wildcard)
                {
                    _cmdDefined = (Command)_unitsFound[0];
                    _state = State.CommandDefined;
                    // prompt for next token here ..
                }
                else
                {
                    _prompt = _unitsFound[0].Name[len..];
                }
            }
        }






        private int RunSearch()
        {
            _unitsFound.Clear();

            int len = 0;
            string wildcard;
            switch (_state)
            {
                case State.Initial:
                {
                    wildcard = _buffer.ToString(_space_start_pos, _pos - _space_start_pos);
                    len = _searchEngine.FindByWildcard(wildcard, _cmdspaces, in _unitsFound);
                    break;
                }

                case State.CommandSpaceDefined:
                {
                    wildcard = _buffer.ToString(_cmd_start_pos, _pos - _cmd_start_pos);
                    len = _searchEngine.FindByWildcard(wildcard, _spaceDefined!.Commands, in _unitsFound);
                    break;
                }

                case State.CommandDefined: // <---------------------------------------------------------------------------------------- Subcommand !???? 
                {
                    wildcard = _buffer.ToString(_cmd_start_pos, _pos - _cmd_start_pos);
                    len = _searchEngine.FindByWildcard(wildcard, _cmdDefined!.Options, in _unitsFound);
                    break;
                }
            }
            return len;
        }


        private void RegisterCommandSpaces()
        {
            _searchEngine.RegisterUnitCollection(_cmdspaces);
            foreach (CommandSpace space in _cmdspaces)
            {
                _searchEngine.RegisterUnitCollection(space.Commands);
                foreach (Command cmd in space.Commands)
                {
                    _searchEngine.RegisterUnitCollection(cmd.Subcommands); // <---------------------------------------------- !!!!! NEED RECURSIVE HERE !!!!
                    _searchEngine.RegisterUnitCollection(cmd.Options);
                }

            }
        }




        //private string GetNextCommandPrompt(string input, CommandSpace space, out Command? cmd)
        //{
        //    cmd = CliUnitSearchEngine.SearchForward(input, space.Commands);
        //    return GetPrompt(cmd, input);
        //}

        //private string GetPreviousCommandPrompt(string input, CommandSpace space, out Command? cmd)
        //{
        //    cmd = CliUnitSearchEngine.SearchBackward(input, space.Commands);
        //    return GetPrompt(cmd, input);
        //}

        //private string GetNextSubcommandPrompt(string input, Command parentCmd, out Command? cmd)
        //{
        //    cmd = CliUnitSearchEngine.SearchForward(input, parentCmd.Subcommands);
        //    return GetPrompt(cmd, input);
        //}

        //private string GetPreviousSubcommandPrompt(string input, Command parentCmd, out Command? cmd)
        //{
        //    cmd = CliUnitSearchEngine.SearchBackward(input, parentCmd.Subcommands);
        //    return GetPrompt(cmd, input);
        //}

        //private string GetNextOptionPrompt(string input, Command cmd)
        //{
        //    var opt = CliUnitSearchEngine.SearchForward(input, cmd.Options);
        //    return GetPrompt(opt, input);
        //}

        //private string GetPreviousOptionPrompt(string input, Command cmd)
        //{
        //    var opt = CliUnitSearchEngine.SearchForward(input, cmd.Options);
        //    return GetPrompt(opt, input);
        //}

        //private string GetHint(string[] input, Command cmd)
        //{
        //    return string.Empty;
        //}

        private string GetPrompt(ISearchableUnit? unit, string input)
        {
            if (unit != null)
                return unit.Name[input.Length..];
            return string.Empty;
        }



        private enum State
        {
            Initial,
            CommandSpaceDefined,
            CommandDefined
        }
    }
}
