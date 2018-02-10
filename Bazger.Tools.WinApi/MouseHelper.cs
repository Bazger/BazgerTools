using System.Runtime.InteropServices;

namespace Bazger.Tools.WinApi
{
    public static class MouseHelper
    {
        /// <summary>
        /// Call the imported function with the cursor's current position
        /// </summary>
        public static void DoMouseLeftClick(int x, int y)
        {
            User32.mouse_event(MOUSEEVENTF.LEFTDOWN | MOUSEEVENTF.LEFTUP, (uint)x, (uint)y, 0, 0);
        }

        public static void DoMouseRightClick(int x, int y)
        {
            User32.mouse_event(MOUSEEVENTF.RIGHTDOWN | MOUSEEVENTF.RIGHTUP, (uint)x, (uint)y, 0, 0);
        }
    }
}
