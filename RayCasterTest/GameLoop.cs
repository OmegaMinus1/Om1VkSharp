using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rayCasterTest
{
    public class GameLoop
    {
        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern bool PeekMessage(
            out Message msg,
            IntPtr hWnd,
            uint messageFilterMin,
            uint messageFilterMax,
            uint flags);
        
        PreciseTimer _timer = new PreciseTimer();
        
        public delegate void LoopCallback(double elapsedTime, double FPS);
        LoopCallback _callback;

        Thread _gameLoopThread = null;

        public GameLoop(LoopCallback callback)
        {
            _callback = callback;

            _gameLoopThread = new Thread(new ThreadStart(TheLoop));
            _gameLoopThread.Start();
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
            return !PeekMessage(out msg, IntPtr.Zero, 0, 0, 0);
        }

    }
}
