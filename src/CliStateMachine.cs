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
        private const ConsoleKey SPACE = ConsoleKey.Spacebar;
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
        private readonly CliUnitCollection _unitsFound;
        private readonly StringBuilder _buffer;

        private State _state;

        private int _tokenStartPosition;
        

        private CommandSpace? _spaceDefined;
        private Command? _cmdDefined;

        private ISearchableUnit? _guess;

        private string _wildcard;
        private string _prompt;


        internal CliStateMachine(IEnumerable<CommandSpace> cmdspaces)
        {
            _cmdspaces = cmdspaces;
            _wildcard = string.Empty;
            _prompt = string.Empty;
            _buffer = new StringBuilder();
            _unitsFound = new CliUnitCollection();
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
                    RemoveLastFromBuffer();
                    UpdateStateOnSymbolOrBackspace();
                    break;

                case (TAB, _):
                    AcceptPrompt();
                    break;

                case (SPACE, _):
                    AddToBuffer(ch);
                    GoToNextToken();
                    break;

                case (ENTR, SHFT):
                    break;

                case (ENTR, _):
                    break;

                case (ESC, _):
                    break;

                case (UARR, _):
                    GetNextPrompt();
                    break;

                case (DARR, _):
                    GetPreviousPrompt();
                    break;

                case (DASH1, _):
                    break;

                case (DASH2, _):
                    break;

                case (_, ALT):
                    break;

                case (_, CTRL):
                    break;

                case (_, CTRL | ALT):
                    break;

                default:
                    AddToBuffer(ch);
                    UpdateStateOnSymbolOrBackspace();
                    break;
            }
        }





        private void AddToBuffer(char ch)
        {
            if (ch >= 32 && ch <= 126)
                _buffer.Append(ch);
        }

        private void RemoveLastFromBuffer()
        {
            if (_buffer.Length == 0)
                return;
            _buffer.Remove(_buffer.Length - 1, 1);
        }

        // for symbol, backspace  (+ mb whitespace ??)
        private void UpdateStateOnSymbolOrBackspace()
        {
            _unitsFound.Clear();


            if (_state == State.Initial)
            {
                string wildcard = _buffer.ToString(0, _buffer.Length);
                int len = _searchEngine.FindByWildcard(wildcard, _cmdspaces, in _unitsFound);
                if (len == 0)
                {
                    return;
                }                
                else if (len == 1 && _unitsFound.GetFirst().Name == wildcard)
                {
                    _spaceDefined = (CommandSpace)_unitsFound.GetFirst();
                    _state = State.CommandSpaceDefined;
                }
                else
                {
                    _guess = _unitsFound.GetCurrent();
                    _prompt = _unitsFound.GetCurrent().Name[wildcard.Length..];
                }
            }



            else if (_state == State.CommandSpaceDefined)
            {
                string wildcard = _buffer.ToString(_tokenStartPosition, _buffer.Length - _tokenStartPosition);
                int len = _searchEngine.FindByWildcard(wildcard, _spaceDefined!.Commands, in _unitsFound);
                if (len == 0)
                {
                    return;
                }
                else if (len == 1 && _unitsFound.GetFirst().Name == wildcard)
                {
                    _cmdDefined = (Command)_unitsFound.GetFirst();
                    _state = State.CommandDefined;
                }
                else
                {
                    _guess = _unitsFound.GetCurrent();
                    _prompt = _unitsFound.GetCurrent().Name[wildcard.Length..];
                }
            }



            else if (_state == State.CommandDefined)
            {
                if (_cmdDefined!.Subcommands.Count > 0)
                {
                    string wildcard = _buffer.ToString(_tokenStartPosition, _buffer.Length - _tokenStartPosition);
                    int len = _searchEngine.FindByWildcard(wildcard, _cmdDefined!.Subcommands, in _unitsFound);
                }
            }

        }


        private void AcceptPrompt()
        {
            if (_prompt != string.Empty)
            {
                switch (_state)
                {
                    case State.Initial:
                        _spaceDefined = (CommandSpace)_guess!;
                        break;


                    case State.CommandSpaceDefined:
                        _cmdDefined = (Command)_guess!;
                        break;

                    case State.CommandDefined:
                        // ????
                        break;                    
                }
                _buffer.Append(_prompt);
            }
        }


        private void GoToNextToken()
        {
            _guess = null;
            _prompt = string.Empty;
            _wildcard = string.Empty;
            _tokenStartPosition = _buffer.Length;
        }
        
        private void GetNextPrompt()
        {
            if (_unitsFound.Count <= 1)
                return;
            var next = _unitsFound.GetNext();
            _guess = next;
            _prompt = next.Name[_wildcard.Length..];
        }

        private void GetPreviousPrompt()
        {
            if (_unitsFound.Count <= 1)
                return;
            var prev = _unitsFound.GetPrevious();
            _guess = prev;
            _prompt = prev.Name[_wildcard.Length..];
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



        private enum State
        {
            Initial,
            CommandSpaceDefined,
            CommandDefined
        }
    }

}
