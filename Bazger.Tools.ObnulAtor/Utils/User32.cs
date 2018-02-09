using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Bazger.Tools.ObnulAtor.Utils
{
    public static class User32
    {
        public const int WM_CLOSE = 16;
        public const int BN_CLICKED = 245;
        public const int WM_COMMAND = 0x0111;
        public const int WM_LBUTTONDOWN = 0x0201;
        public const int WM_LBUTTONUP = 0x0202;
        public const int IDOK = 1;
        public const int WM_KEYDOWN = 0x100;
        public const int WM_KEYUP = 0x101;
        public const int WM_CHAR = 0x105;
        public const int WM_SYSKEYDOWN = 0x104;
        public const int WM_SYSKEYUP = 0x105;

        public const int WM_USER = 0x0400;

        public const int TB_HIDEBUTTON = WM_USER + 4;

        public const int TB_DELETEBUTTON = WM_USER + 22;
        public const int TB_BUTTONCOUNT = WM_USER + 24;
        public const int TB_GETBUTTON = WM_USER + 23;
        public const int TB_GETBUTTONINFO = WM_USER + 63;

        public const int TBIF_IMAGE = 0x0001;
        public const int TBIF_TEXT = 0x0002;
        public const int TBIF_STATE = 0x0004;
        public const int TBIF_STYLE = 0x0008;
        public const int TBIF_LPARAM = 0x0010;
        public const int TBIF_COMMAND = 0x0020;
        public const int TBIF_SIZE = 0x0040;
        public const uint TBIF_BYINDEX = 0x80000000;


        // A delegate which is used by EnumChildWindows to execute a callback method.
        public delegate bool EnumWindowProc(IntPtr hWnd, IntPtr parameter);


        [DllImport("user32")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool EnumChildWindows(IntPtr window, EnumWindowProc callback, IntPtr i);

        [DllImport("user32.dll", EntryPoint = "GetWindowText", CharSet = CharSet.Unicode)]
        public static extern IntPtr GetWindowCaption(IntPtr hwnd, StringBuilder lpString, int maxCount);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern bool EnumWindows(EnumWindowProc enumProc, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern IntPtr GetDesktopWindow();

        [StructLayout(LayoutKind.Sequential)]
        public struct TBBUTTONINFO
        {
            public uint cbSize;
            public uint dwMask;
            public int idCommand;
            public int iImage;
            public byte fsState;
            public byte fsStyle;
            public short cx;
            public IntPtr lParam;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpszText;
            public int cchText;
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

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool RedrawWindow(IntPtr hWnd, IntPtr lprcUpdate, IntPtr hrgnUpdate, uint flags);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint msg, int wParam, bool lParam);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint msg, int wParam, int lParam);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint msg, int wParam, ref TBBUTTONINFO lParam);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint msg, int wParam, out TBBUTTON lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SendMessage(IntPtr hWnd, int msg, int wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int PostMessage(IntPtr hWnd, int msg, int wParam, IntPtr lParam);

    }
}
