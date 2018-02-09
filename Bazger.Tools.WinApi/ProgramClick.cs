using System.Runtime.InteropServices;

namespace Bazger.Tools.WinApi
{
    public static class ProgramClick
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);

        /// <summary>
        /// Call the imported function with the cursor's current position
        /// </summary>
        public static void DoMouseLeftClick(int x, int y)
        {
            mouse_event(Constants.MOUSEEVENTF_LEFTDOWN | Constants.MOUSEEVENTF_LEFTUP, (uint)x, (uint)y, 0, 0);
        }

        public static void DoMouseRightClick(int x, int y)
        {
            mouse_event(Constants.MOUSEEVENTF_RIGHTDOWN | Constants.MOUSEEVENTF_RIGHTUP, (uint)x, (uint)y, 0, 0);
        }
    }
}
