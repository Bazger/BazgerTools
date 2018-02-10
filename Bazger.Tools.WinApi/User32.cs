using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Bazger.Tools.WinApi
{
    public enum GW : UInt32
    {
        HWNDFIRST = 0,
        HWNDLAST = 1,
        HWNDNEXT = 2,
        HWNDPREV = 3,
        OWNER = 4,
        CHILD = 5,
        MAX = 6
    }

    public static class ICON
    {
        public const UInt32 SMALL = 0;
        public const UInt32 BIG = 1;
        public const UInt32 SMALL2 = 2; // XP+
    }

    public enum MB : uint
    {
        SimpleBeep = 0xFFFFFFFF,
        IconAsterisk = 0x00000040,
        IconWarning = 0x00000030,
        IconError = 0x00000010,
        IconQuestion = 0x00000020,
        OK = 0x00000000
    }

    public static class SW
    {
        public const int HIDE = 0;
        public const int SHOWNORMAL = 1;
        public const int NORMAL = 1;
        public const int SHOWMINIMIZED = 2;
        public const int SHOWMAXIMIZED = 3;
        public const int MAXIMIZE = 3;
        public const int SHOWNOACTIVATE = 4;
        public const int SHOW = 5;
        public const int MINIMIZE = 6;
        public const int SHOWMINNOACTIVE = 7;
        public const int SHOWNA = 8;
        public const int RESTORE = 9;
        public const int SHOWDEFAULT = 10;
        public const int FORCEMINIMIZE = 11;
        public const int MAX = 11;
    }

    public static class BN
    {
        public const int CLICKED = 245;
    }

    public static class TB
    {
        public const uint GETBUTTON = WM.USER + 23;
        public const uint BUTTONCOUNT = WM.USER + 24;
        public const uint CUSTOMIZE = WM.USER + 27;
        public const uint GETBUTTONTEXTA = WM.USER + 45;
        public const uint GETBUTTONTEXTW = WM.USER + 75;
        public const uint HIDEBUTTON = WM.USER + 4;
        public const uint DELETEBUTTON = WM.USER + 22;
        public const uint GETBUTTONINFO = WM.USER + 63;
    }

    public static class TBIF
    {
        public const uint IMAGE = 0x0001;
        public const uint TEXT = 0x0002;
        public const uint STATE = 0x0004;
        public const uint STYLE = 0x0008;
        public const uint LPARAM = 0x0010;
        public const uint COMMAND = 0x0020;
        public const uint SIZE = 0x0040;
        public const uint BYINDEX = 0x80000000;
    }

    public static class TBSTATE
    {
        public const uint CHECKED = 0x01;
        public const uint PRESSED = 0x02;
        public const uint ENABLED = 0x04;
        public const uint HIDDEN = 0x08;
        public const uint INDETERMINATE = 0x10;
        public const uint WRAP = 0x20;
        public const uint ELLIPSES = 0x40;
        public const uint MARKED = 0x80;
    }

    public static class WM
    {
        public const uint CLOSE = 0x0010;
        public const uint GETICON = 0x007F;
        public const uint KEYDOWN = 0x0100;
        public const uint KEYUP = 0x0101;
        public const uint COMMAND = 0x0111;
        public const uint USER = 0x0400; // 0x0400 - 0x7FFF
        public const uint APP = 0x8000; // 0x8000 - 0xBFFF
        public const uint LBUTTONDOWN = 0x0201;
        public const uint LBUTTONUP = 0x0202;
        public const uint CHAR = 0x105;
        public const uint SYSKEYDOWN = 0x104;
        public const uint SYSKEYUP = 0x105;
        public const uint HOTKEY_MSG_ID = 0x0312; //windows message id for hotkey
    }

    public static class GCL
    {
        public const int MENUNAME = -8;
        public const int HBRBACKGROUND = -10;
        public const int HCURSOR = -12;
        public const int HICON = -14;
        public const int HMODULE = -16;
        public const int CBWNDEXTRA = -18;
        public const int CBCLSEXTRA = -20;
        public const int WNDPROC = -24;
        public const int STYLE = -26;
        public const int ATOM = -32;
        public const int HICONSM = -34;

        // GetClassLongPtr ( 64-bit )
        private const int GCW_ATOM = -32;
        private const int GCL_CBCLSEXTRA = -20;
        private const int GCL_CBWNDEXTRA = -18;
        private const int GCLP_MENUNAME = -8;
        private const int GCLP_HBRBACKGROUND = -10;
        private const int GCLP_HCURSOR = -12;
        private const int GCLP_HICON = -14;
        private const int GCLP_HMODULE = -16;
        private const int GCLP_WNDPROC = -24;
        private const int GCLP_HICONSM = -34;
        private const int GCL_STYLE = -26;

    }

    public static class ModifierKeys
    {
        public const int NOMOD = 0x0000;
        public const int ALT = 0x0001;
        public const int CTRL = 0x0002;
        public const int SHIFT = 0x0004;
        public const int WIN = 0x0008;
    }

    public static class MOUSEEVENTF
    {
        public const int LEFTUP = 0x04;
        public const int LEFTDOWN = 0x02;
        public const int RIGHTDOWN = 0x08;
        public const int RIGHTUP = 0x10;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct TBBUTTON
    {
        public Int32 iBitmap;
        public Int32 idCommand;
        public byte fsState;
        public byte fsStyle;
        //      [ MarshalAs( UnmanagedType.ByValArray, SizeConst=2 ) ]
        //      public byte[] bReserved;
        public byte bReserved1;
        public byte bReserved2;
        // public UInt32 dwData;
        public UInt64 dwData;
        public IntPtr iString;
    };

    [StructLayout(LayoutKind.Sequential)]
    public struct TBBUTTONINFO
    {
        public UInt32 cbSize;
        public UInt32 dwMask;
        public Int32 idCommand;
        public Int32 iImage;
        public byte fsState;
        public byte fsStyle;
        public short cx;
        public IntPtr lParam;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string lpszText;
        public Int32 cchText;
    }

    /// <summary>
    /// This class I received forum
    /// For more help with usage see link: https://stackoverflow.com/questions/33652756/how-to-get-the-processes-that-have-systray-icon/33707955
    /// </summary>
    public static class User32
    {
        // A delegate which is used by EnumChildWindows to execute a callback method.
        public delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr parameter);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern IntPtr SendMessage(IntPtr hWnd, UInt32 msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern IntPtr SendMessage(IntPtr hWnd, UInt32 msg, Int32 wParam, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr SendMessage(IntPtr hWnd, UInt32 msg, Int32 wParam, bool lParam);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr SendMessage(IntPtr hWnd, UInt32 msg, Int32 wParam, Int32 lParam);

        [DllImport("user32.dll")]
        public static extern UInt32 SendMessage(IntPtr hWnd, UInt32 msg, UInt32 wParam, UInt32 lParam);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr SendMessage(IntPtr hWnd, UInt32 msg, Int32 wParam, ref TBBUTTONINFO lParam);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr SendMessage(IntPtr hWnd, UInt32 msg, Int32 wParam, out TBBUTTON lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern Int32 SendMessage(IntPtr hWnd, Int32 msg, Int32 wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern Int32 PostMessage(IntPtr hWnd, Int32 msg, Int32 wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern Int32 PostMessage(IntPtr hWnd, UInt32 msg, Int32 wParam, IntPtr lParam);


        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool PostMessage
        (IntPtr hWnd, UInt32 msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern bool PostMessage
        (
            IntPtr hWnd,
            UInt32 msg,
            UInt32 wParam,
            UInt32 lParam
        );

        [DllImport("user32.dll")]
        public static extern bool MessageBeep
        (
            MB beepType
        );

        [DllImport("user32.dll")]
        public static extern bool ShowWindow
        (
            IntPtr hWnd,
            Int32 nCmdShow
        );

        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow
        (
            IntPtr hWnd
        );


        [DllImport("user32.dll")]
        public static extern IntPtr GetDesktopWindow();


        [DllImport("user32.dll")]
        public static extern IntPtr GetWindow
        (
            IntPtr hWnd,
            GW uCmd
        );

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool RedrawWindow(IntPtr hWnd, IntPtr lprcUpdate, IntPtr hrgnUpdate, UInt32 flags);


        [DllImport("user32")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool EnumChildWindows(IntPtr window, EnumWindowsProc callback, IntPtr i);

        [DllImport("user32.dll", EntryPoint = "GetWindowText", CharSet = CharSet.Unicode)]
        public static extern IntPtr GetWindowCaption(IntPtr hwnd, StringBuilder lpString, Int32 maxCount);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern Int32 GetWindowTextLength
        (
            IntPtr hWnd
        );

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern Int32 GetWindowText
        (
            IntPtr hWnd,
            out StringBuilder lpString,
            Int32 nMaxCount
        );

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern Int32 GetClassName
        (
            IntPtr hWnd,
            out StringBuilder lpClassName,
            Int32 nMaxCount
        );

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool BringWindowToTop(IntPtr hwnd);

        //      [ DllImport( "user32.dll", EntryPoint = "GetClassLongPtrW" ) ]
        [DllImport("user32.dll")]
        public static extern UInt32 GetClassLong
        (
            IntPtr hWnd,
            Int32 nIndex
        );

        [DllImport("user32.dll")]
        public static extern UInt32 SetClassLong
        (
            IntPtr hWnd,
            Int32 nIndex,
            UInt32 dwNewLong
        );

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern UInt32 GetWindowThreadProcessId
        (
            IntPtr hWnd,
            out UInt32 lpdwProcessId
        );

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern IntPtr FindWindowEx(
            IntPtr hwndParent,
            IntPtr hwndChildAfter,
            string lpszClass,
            string lpszWindow);

        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr FindWindowByClass(string lpClassName, IntPtr zero);

        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr FindWindowByCaption(IntPtr zero, string lpWindowName);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool EnumChildWindows(IntPtr hWndParent, EnumWindowsProc lpEnumFunc, int lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);

        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);

        [DllImport("user32.dll")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

    }
}
