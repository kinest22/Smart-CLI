#pragma warning disable SYSLIB1054
#pragma warning disable IDE1006

using System;
using System.Runtime.InteropServices;

namespace SmartCLI
{
    /// <summary>
    ///     Wrapper class over native Windows console API functions.
    /// </summary>
    public static class NativeConsoleApi
    {
        private const int STD_OUTPUT_HANDLE = -11;
        private static readonly nint _consoleHandle = default;

        static NativeConsoleApi()        
            => _consoleHandle = GetStdHandle(STD_OUTPUT_HANDLE);
        
        /// <summary>
        ///     Prints chars to console screen buffer (stdout) at specified position.
        /// </summary>
        /// <param name="chars">sequence of chars to be printed.</param>
        /// <param name="x">row from top.</param>
        /// <param name="y">col from left.</param>
        /// <param name="attrval">attributes bitmask.</param>
        /// <returns>
        ///     number of chars written.
        /// </returns>
        public static uint Print(string chars, short x, short y, ushort attrval)
        {
            ushort[] attr = new ushort[chars.Length];
            Array.Fill(attr, attrval, 0, chars.Length);
            var coord = new COORD(x, y);
            WriteConsoleOutputAttribute(_consoleHandle, attr, (uint)chars.Length, coord, out uint _);
            WriteConsoleOutputCharacter(_consoleHandle, chars, (uint)chars.Length, coord, out uint written);
            return written;
        }

        /// <summary>
        ///     Retrieves a handle to the specified standard device 
        ///     (standard input, standard output, or standard error).
        /// </summary>
        /// 
        /// <param name="nStdHandle">
        ///     The standard device. stdin: -10; stout: -11; stderr: -12
        /// </param>
        /// 
        /// <returns>
        ///     If the function succeeds, the return value is a handle to the specified device.
        /// </returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern nint GetStdHandle(int nStdHandle);



        /// <summary>
        ///     Native (Win) Console API function to write attributes to console screen at specified position.
        /// </summary>
        /// 
        /// <param name="hConsoleOutput">
        ///     A handle to the console screen buffer.
        /// </param>
        /// 
        /// <param name="lpAttribute">
        ///     The attributes to be used when writing to the console screen buffer. 
        /// </param>
        /// 
        /// <param name="nLength">
        ///     The number of screen buffer character cells to which the attributes will be copied.
        /// </param>
        /// 
        /// <param name="dwWriteCoord">
        ///     A <see cref="COORD"/> structure that specifies the character coordinates of the first cell 
        ///     in the console screen buffer to which the attributes will be written.
        /// </param>
        /// 
        /// <param name="lpNumberOfCharsWritten">
        ///     A pointer to a variable that receives the number of attributes actually written to the console 
        ///     screen buffer.
        /// </param>
        /// 
        /// <returns>
        ///     If the function succeeds, the return value is nonzero.
        /// </returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool WriteConsoleOutputAttribute(nint hConsoleOutput, ushort[] lpAttribute, 
                uint nLength, COORD dwWriteCoord, out uint lpNumberOfCharsWritten);



        /// <summary>
        ///    Native (Win) Console API function to print chars to console screen at specified position. 
        /// </summary>
        /// 
        /// <param name="hConsoleOutput">
        ///     A handle to the console screen buffer.
        /// </param>
        /// 
        /// <param name="lpCharacter">
        ///     The characters to be written to the console screen buffer.
        /// </param>
        /// 
        /// <param name="nLength">
        ///     The number of characters to be written.
        /// </param>
        /// 
        /// <param name="dwWriteCoord">
        ///     A <see cref="COORD"/> structure that specifies the character coordinates of the first cell 
        ///     in the console screen buffer to which characters will be written.
        /// </param>
        /// 
        /// <param name="lpNumberOfCharsWritten">
        ///     A pointer to a variable that receives the number of characters actually written.
        /// </param>
        /// 
        /// <returns>
        ///     If the function succeeds, the return value is nonzero.
        /// </returns>
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern bool WriteConsoleOutputCharacter(nint hConsoleOutput, string lpCharacter,                
                uint nLength, COORD dwWriteCoord, out uint lpNumberOfCharsWritten);
    }
}