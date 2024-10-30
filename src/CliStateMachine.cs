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
        private readonly Dictionary<ICliUnit, string> _opts;                    // command opstions
        private readonly List<string> _args;                                    // command args
        private readonly StringBuilder _buffer;                                 // input buffer


        private IEnumerable<ICliUnit> _units;                                   // current collection of unit available to search within

        private State _state;                                                   // current machine state

        private int _tokenStartPosition;                                        // pending token start position      


        private ICliUnit? _unitGuess;                                           // current CLI unit guess
        private ICliUnit? _spaceDefined;                                        // command space already defined
        private ICliCommand<ICliUnit>? _cmdDefined;                             // command already defined
        private ICliUnit? _optDefined;                                          // command option already defined

        private string _prompt;                                                 // current prompt according to wildcard 


        internal CliStateMachine(CliCommandHierarchy hierarchy)
        {
            _searchEngine = CliUnitSearchEngine.Create();
            _unitsFound = new CliUnitSearchResults();
            _opts = new Dictionary<ICliUnit, string>();
            _args = new List<string>();
            _buffer = new StringBuilder();
            _prompt = string.Empty;
            _units = hierarchy;
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
            _prompt = string.Empty;
        }


        /// <summary>
        ///     Registers all available commands in CLI environment for futher access.
        /// </summary>
        /// <param name="units">Collection of <see cref="ICliUnit"/>s.</param>
        internal void RegisterCommandHierarchy(IEnumerable<ICliUnit> units)
        {
            _searchEngine.RegisterUnitCollection(units);
            foreach (var unit in units)
                if (unit.SubUnits.Any())
                    RegisterCommandHierarchy(unit.SubUnits);
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
                    CompleteToken();
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
        ///     This method is called internally in case of any symbol entered.
        /// </summary>
        private void AddToBuffer(char ch)
        {
            if (ch >= 32 && ch <= 126)
                _buffer.Append(ch);
        }


        /// <summary>
        ///     Removes last available char from CLI input buffer.
        ///     This method is called internally in case of BACKSPACE entered.
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
            // defining current wildcard
            string wildcard = GetCurrentWildcard();

            // following logic describes the way machine state 
            // is changed depending on current wildcard

            if (TryDefineUnit(wildcard, _units, out var unit))
            {
                // if current wildcard fully coincides any of
                // CLI unit names currently available
                // then either cmd-space or cmd or cmd option 
                // is fully defined.

                // redefine current collection of CLI units
                _units = unit!.SubUnits;

                if (_state == State.Started)
                {
                    // command space is now fully defined
                    _spaceDefined = unit;
                    _state = State.CommandSpaceDefined;
                }

                else if (_state == State.CommandSpaceDefined)
                {
                    // command is now fully defined
                    _cmdDefined = (ICliCommand<ICliUnit>?)unit;
                    _state = State.CommandDefined;
                }

                else if (_state == State.CommandDefined)
                {
                    // either subcmd or option is now fully defined
                    if (unit!.IsParameter) 
                    {
                        // option defined
                        _optDefined = unit;
                        _state = State.OptionDefined;
                    }
                    else
                    {
                        // subcmd defined
                        // no state change here
                        _cmdDefined = (ICliCommand<ICliUnit>?)unit;
                    }
                }                

                CompleteToken();
            }

            // prompt should be defined.
            // if input does not coincides any of unit names
                // then wildcard is either value for option
                // or argument of command

                if (!TryDefinePrompt(wildcard, out _prompt))
                {
                    // current wildcard is not a part of any
                    // CLI unit name currently abailable

                    if (_state == State.CommandDefined)
                    {
                        // current wildcard is command argument value

                    }
                    else if (_state == State.OptionDefined)
                    {
                        // current wildcard is option value

                    }
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
        private void CompleteToken()
        {
            _unitGuess = null;
            _prompt = string.Empty;
            _tokenStartPosition = _buffer.Length;
            
            if (_state == State.CommandDefined)
            {
                // current wildcard is command argument value

            }
            else if (_state == State.OptionDefined)
            {
                // current wildcard is option value

            }
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
            string wildcard = GetCurrentWildcard();
            _prompt = next.Name[wildcard.Length..];
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
            string wildcard = GetCurrentWildcard();
            _prompt = prev.Name[wildcard.Length..];
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
        /// <param name="wildcard">Wildcard.</param>
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
        ///     Tries to define prompt according to specified wildcard.
        /// </summary>
        /// <param name="wildcard">Wildcard.</param>
        /// <param name="prompt">Pointer to <see cref="string"/> for prompt defined </param>
        /// <returns>TRUE if prompt was successfully defined, otherwise - FALSE</returns>
        private bool TryDefinePrompt(string wildcard, out string prompt)
        {
            if (!_unitsFound.Any())
            {
                prompt = string.Empty;
                return false;
            }

            ICliUnit currentGuess = _unitsFound.GetCurrent();
            if (currentGuess.Name.StartsWith(wildcard))
            {
                prompt = currentGuess.Name[wildcard.Length..];
                return true;
            }
            prompt = string.Empty;
            return false;
        }
    }
}
