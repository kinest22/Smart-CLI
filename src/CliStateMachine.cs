using SmartCLI.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartCLI
{
    internal sealed class CliStateMachine
    {
        internal enum State
        {
            Started,
            CommandSpaceDefined,
            CommandDefined,
            OptionDefined,
            Completed,
            InputAborted,
            CancellationRequested
        }

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


        private readonly CliUnitSearchEngine _searchEngine;
        private readonly CliUnitSearchResults _unitsFound;
        private readonly StringBuilder _buffer;


        private IEnumerable<ICliUnit> _units;
        private State _state;

        private int _tokenStartPosition;
        


        private ICliUnit? _unitGuess;
        private ICliUnit? _spaceDefined;
        private ICliUnit? _cmdDefined;
        private ICliUnit? _optDefined;

        private string _wildcard;
        private string _prompt;


        internal CliStateMachine(CliCommandHierarchy hierarchy)
        {
            _wildcard = string.Empty;
            _prompt = string.Empty;
            _buffer = new StringBuilder();
            _unitsFound = new CliUnitSearchResults();
            _searchEngine = CliUnitSearchEngine.Create();
            _units = hierarchy;
            RegisterCommandHierarchy(hierarchy);
        }



        internal void Reset()
        {
            _unitsFound.Clear();
            _buffer.Clear();
            _tokenStartPosition = 0;
            _spaceDefined = default;
            _cmdDefined = default;
            _unitGuess = default;
            _wildcard = string.Empty;
            _prompt = string.Empty;
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
                    UpdateState();
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
                    Complete();
                    break;

                case (ESC, _):
                    StopMachine();
                    break;

                case (UARR, _):
                    GetNextPrompt();
                    break;

                case (DARR, _):
                    GetPreviousPrompt();
                    break;

                case (_, ALT):
                    break;

                case (_, CTRL):
                    break;

                case (_, CTRL | ALT):
                    break;

                default:
                    AddToBuffer(ch);
                    UpdateState();
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


        private void UpdateState()
        {
            string wildcard = GetCurrentWildcard();


            if (TryDefineUnit(wildcard, _units, out var unit))
            {
                // redefine current collection of CLI units
                _units = unit!.SubUnits;

                // command space is now fully defined
                if (_state == State.Started)
                {
                    _spaceDefined = unit;
                    _state = State.CommandSpaceDefined;
                }

                // command is now fully defined
                else if (_state == State.CommandSpaceDefined)
                {
                    _cmdDefined = unit;
                    _state = State.CommandDefined;
                }

                // either subcmd or option is now fully defined
                else if (_state == State.CommandDefined)
                {
                    // option defined
                    if (unit!.IsParameter) 
                    {
                        _optDefined = unit;
                        _state = State.OptionDefined;
                    }
                    // subcmd defined
                    else
                    {
                        _cmdDefined = unit;
                    }
                }

                else if (_state == State.OptionDefined)
                {

                }
            }
            else
            {
                _prompt = DefinePrompt(wildcard);
            }
        }


        private bool TryDefineUnit(string wildcard, IEnumerable<ICliUnit> units, out ICliUnit? unitDefined)
        {
            _unitsFound.Clear();
            int len = _searchEngine.FindByWildcard(wildcard, units, in _unitsFound);
            if (len == 1 && _unitsFound.GetFirst().Name == wildcard)
            {
                unitDefined = _unitsFound.GetFirst();
                return true;
            }
            else
            {
                unitDefined = null;
                return false;
            }
        }


        private string DefinePrompt(string wildcard)
        {
            ICliUnit currentGuess = _unitsFound.GetCurrent();
            return currentGuess.Name[wildcard.Length..];
        }



        private void AcceptPrompt()
        {
            if (_prompt != string.Empty)
            {
                switch (_state)
                {
                    case State.Started:
                        _spaceDefined = _unitGuess!;
                        break;


                    case State.CommandSpaceDefined:
                        _cmdDefined = _unitGuess!;
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
            _unitGuess = null;
            _prompt = string.Empty;
            _wildcard = string.Empty;
            _tokenStartPosition = _buffer.Length;
        }
        
        private void GetNextPrompt()
        {
            if (_unitsFound.Count <= 1)
                return;
            var next = _unitsFound.GetNext();
            _unitGuess = next;
            _prompt = next.Name[_wildcard.Length..];
        }

        private void GetPreviousPrompt()
        {
            if (_unitsFound.Count <= 1)
                return;
            var prev = _unitsFound.GetPrevious();
            _unitGuess = prev;
            _prompt = prev.Name[_wildcard.Length..];
        }

        private void StopMachine()
        {
            _state = _state == State.Completed 
                ? State.CancellationRequested 
                : State.InputAborted;
        }

        private void Complete()
        {
            _state = State.Completed;
        }


        private void RegisterCommandHierarchy(IEnumerable<ICliUnit> units)
        {
            _searchEngine.RegisterUnitCollection(units);
            foreach (var unit in units)
                if (unit.SubUnits.Any())
                    RegisterCommandHierarchy(unit.SubUnits);
        }

        private string GetCurrentWildcard()
        {
            int startIndex = _tokenStartPosition;
            int length = _buffer.Length - _tokenStartPosition;
            return _buffer.ToString(startIndex, length);
        }



        
    }

}
