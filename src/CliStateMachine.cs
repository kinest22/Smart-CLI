using SmartCLI.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartCLI
{
    /// <summary>
    ///     CLI Finite State Machine for CLI input processing.
    /// </summary>
    internal sealed class CliStateMachine
    {
        /// <summary>
        ///     CLI state machine possible states.
        /// </summary>
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
        private const ConsoleKey BKSP           = ConsoleKey.Backspace;
        private const ConsoleKey TAB            = ConsoleKey.Tab;
        private const ConsoleKey SPACE          = ConsoleKey.Spacebar;
        private const ConsoleKey ENTR           = ConsoleKey.Enter;
        private const ConsoleKey ESC            = ConsoleKey.Escape;
        private const ConsoleKey UARR           = ConsoleKey.UpArrow;
        private const ConsoleKey DARR           = ConsoleKey.DownArrow;
        private const ConsoleKey DASH1          = ConsoleKey.Subtract;
        private const ConsoleKey DASH2          = ConsoleKey.OemMinus;
        private const ConsoleModifiers ALT      = ConsoleModifiers.Alt;
        private const ConsoleModifiers SHFT     = ConsoleModifiers.Shift;
        private const ConsoleModifiers CTRL     = ConsoleModifiers.Control;
#pragma warning restore IDE1006 // Naming Styles


        private readonly CliUnitSearchEngine _searchEngine;                     // CLI Search Engine used to search CLI units
        private readonly CliUnitSearchResults _unitsFound;                      // cycled collection of CLI unts found
        private readonly StringBuilder _buffer;                                 // input buffer


        private IEnumerable<ICliUnit> _units;                                   // current collection of unit available to search within

        private State _state;                                                   // current machine state

        private int _tokenStartPosition;                                        // pending token start position      


        private ICliUnit? _unitGuess;                                           // current CLI unit guess
        private ICliUnit? _spaceDefined;                                        // command space already defined
        private ICliUnit? _cmdDefined;                                          // command already defined
        private ICliUnit? _optDefined;                                          // command option already defined

        private string _wildcard;                                               // current wildcard from buffer
        private string _prompt;                                                 // current prompt according to wildcard 


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


        /// <summary>
        ///     Resets state machine.
        /// </summary>
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

        /// <summary>
        ///     Processes specified input.
        /// </summary>
        /// <param name="cki"><see cref="ConsoleKeyInfo"/> entered.</param>
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
                    UpdateState();
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
                    GetPreviousPrompt();
                    break;

                case (DARR, _):
                    GetNextPrompt();
                    break;

                case (_, ALT):
                    // ignore any ALT + smth inputs
                    break;

                case (_, CTRL):
                    // ignore any CTRL + smth inputs
                    break;

                case (_, CTRL | ALT):
                    // ignore any CTRL + ALT + smth inputs
                    break;

                default:
                    AddToBuffer(ch);
                    UpdateState();
                    break;
            }
        }

        /// <summary>
        ///     Adds specified char to CLI input buffer.
        /// </summary>
        /// <param name="ch"></param>
        private void AddToBuffer(char ch)
        {
            if (ch >= 32 && ch <= 126)
                _buffer.Append(ch);
        }

        /// <summary>
        ///     Removes last available char from CLI input buffer.
        /// </summary>
        private void RemoveLastFromBuffer()
        {
            if (_buffer.Length == 0)
                return;
            _buffer.Remove(_buffer.Length - 1, 1);
        }

        /// <summary>
        ///     Updates state of machine depending on current CLI input buffer content. 
        ///     This method is called internally in case of BACKSPACE or any symbol entered.
        /// </summary>
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

        /// <summary>
        ///     Accepts current prompt (if any).
        ///     This method is called internaly in case of TAB pressed.
        /// </summary>
        private void AcceptPrompt()
        {
            if (_prompt != string.Empty)                           
                _buffer.Append(_prompt);            
        }

        /// <summary>
        ///     Changes current token start position in CLI buffer. 
        ///     This method is called internaly in case of SPACE pressed.
        /// </summary>
        private void GoToNextToken()
        {
            _unitGuess = null;
            _prompt = string.Empty;
            _wildcard = string.Empty;
            _tokenStartPosition = _buffer.Length;
        }

        /// <summary>
        ///     Switches current prompt to the next one (if any). 
        ///     This method is called internaly in case of DOWN_ARROW pressed.     
        /// </summary>
        private void GetNextPrompt()
        {
            if (_unitsFound.Count <= 1)
                return;
            var next = _unitsFound.GetNext();
            _unitGuess = next;
            _prompt = next.Name[_wildcard.Length..];
        }

        /// <summary>
        ///     Switches current prompt to the previous one (if any). 
        ///     This method is called internaly in case of UP_ARROW pressed.     
        /// </summary>
        private void GetPreviousPrompt()
        {
            if (_unitsFound.Count <= 1)
                return;
            var prev = _unitsFound.GetPrevious();
            _unitGuess = prev;
            _prompt = prev.Name[_wildcard.Length..];
        }

        /// <summary>
        ///     Stops machine's <see cref="ConsoleKeyInfo"/> processing procedure.
        ///     This method is called internaly in case of ESCAPE pressed.  
        /// </summary>
        private void StopMachine()
        {
            _state = _state == State.Completed 
                ? State.CancellationRequested 
                : State.InputAborted;
        }

        /// <summary>
        ///     Stops machine's <see cref="ConsoleKeyInfo"/> processing procedure.
        ///     This method is called internaly in case of ENTER pressed.  
        /// </summary>
        private void Complete()
        {
            _state = State.Completed;
        }

        /// <summary>
        ///     Registers all available commands in CLI environment for futher access.
        /// </summary>
        /// <param name="units">Collection of <see cref="ICliUnit"/>s.</param>
        private void RegisterCommandHierarchy(IEnumerable<ICliUnit> units)
        {
            _searchEngine.RegisterUnitCollection(units);
            foreach (var unit in units)
                if (unit.SubUnits.Any())
                    RegisterCommandHierarchy(unit.SubUnits);
        }

        /// <summary>
        ///     Gets current wildcard according to CLI input buffer contents.
        /// </summary>
        /// <returns></returns>
        private string GetCurrentWildcard()
        {
            int startIndex = _tokenStartPosition;
            int length = _buffer.Length - _tokenStartPosition;
            return _buffer.ToString(startIndex, length);
        }

        /// <summary>
        ///     Tries to define <see cref="ICliUnit"/> in specified <see cref="ICliUnit"/> collection 
        ///     according to specified wildcard.
        /// </summary>
        /// <param name="wildcard"></param>
        /// <param name="units">Collection of <see cref="ICliUnit"/> from which unit should be defined.</param>
        /// <param name="unitDefined">Pointer to <see cref="ICliUnit"/> instance for unit defined.</param>
        /// <returns>TRUE if <see cref="ICliUnit"/> was successfully defined, otherwise - FALSE</returns>
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

        /// <summary>
        ///     Defines prompt according to specified wildcard.
        /// </summary>
        /// <param name="wildcard"></param>
        /// <returns></returns>
        private string DefinePrompt(string wildcard)
        {
            ICliUnit currentGuess = _unitsFound.GetCurrent();
            return currentGuess.Name[wildcard.Length..];
        }
    }

}
