using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Bazger.Tools.WinApi
{
    public class GlobalHotkey
    {
        private readonly int _modifier;
        private readonly int _key;
        private readonly IntPtr _hWnd;
        private readonly int _id;

        public GlobalHotkey(int id, int modifier, Keys key, Form form)
        {
            this._modifier = modifier;
            this._key = (int)key;
            this._hWnd = form.Handle;
            this._id = id;
        }

        public bool Register()
        {
            return User32.RegisterHotKey(_hWnd, _id, _modifier, _key);
        }

        public bool Unregiser()
        {
            return User32.UnregisterHotKey(_hWnd, _id);
        }

        public override int GetHashCode()
        {
            return _modifier ^ _key ^ _hWnd.ToInt32();
        }
    }
}
