using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SmartCLI
{
    /// <summary>
    ///     Defines the coordinates of a character cell in a console screen buffer. The origin of the coordinate system (0,0) is at the top, left cell of the buffer.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal readonly struct COORD
    {
        public readonly short x; // row number from current pos 
        public readonly short y; // col number from current pos

        public COORD(short x, short y)
        {
            this.x = x; 
            this.y = y;
        }

    }
}
