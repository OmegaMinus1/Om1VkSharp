using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShaderGenesis
{
    public class GameLoop
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct Win32Message
        {
            public IntPtr hwnd;
            public uint message;
            public IntPtr wParam;
            public IntPtr lParam;
            public uint time;
            public System.Drawing.Point point;
        }

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("user32.dll")]
        public static extern bool TranslateMessage([In] ref Win32Message lpMsg);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("user32.dll")]
        public static extern IntPtr DispatchMessage([In] ref Win32Message lpmsg);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern bool PeekMessage(
            out Win32Message msg,
            IntPtr hWnd,
            uint messageFilterMin,
            uint messageFilterMax,
            uint flags);
        
        static public PreciseTimer _timer = new PreciseTimer();

        Win32Message msg;

        public delegate void LoopCallback(double elapsedTime, double FPS);
        LoopCallback _callback;

        uint PM_REMOVE = 0x0001;

        Thread _gameLoopThread = null;

        public GameLoop(LoopCallback callback)
        {
            _callback = callback;

            msg = new Win32Message();

            Application.Idle += Application_Idle;

            //_gameLoopThread = new Thread(new ThreadStart(TheLoop));
            //_gameLoopThread.Start();
        }

        private void Application_Idle(object sender, EventArgs e)
        {

            Win32Message msg;
            if (PeekMessage(out msg, IntPtr.Zero, 0, 0, PM_REMOVE))
            {
                TranslateMessage(ref msg);
                DispatchMessage(ref msg);
            }
            else
            {
                _callback(_timer.GetElapsedTime(), _timer.GetFPS());
            }
           
        }

        void TheLoop()
        {
            while (IsAppRunning())
            {
                _callback(_timer.GetElapsedTime(), _timer.GetFPS());
            }
            
            Application.Exit();
            Thread.CurrentThread.Abort();
        }

        private bool IsAppRunning()
        {
            Message msg;
            //return !PeekMessage(out msg, IntPtr.Zero, 0, 0, 0);
            return true;
        }

    }
}
