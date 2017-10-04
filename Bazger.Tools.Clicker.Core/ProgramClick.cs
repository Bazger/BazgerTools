﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bazger.Tools.Clicker.Core
{
    public static class ProgramClick
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);

        /// <summary>
        /// Call the imported function with the cursor's current position
        /// </summary>
        public static void DoMouseClick(int x, int y)
        {
            mouse_event(Constants.MOUSEEVENTF_LEFTDOWN | Constants.MOUSEEVENTF_LEFTUP, (uint)x, (uint)y, 0, 0);
        }
    }
}
