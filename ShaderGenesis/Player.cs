using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShaderGenesis
{
    public class Player
    {
        public double PosX;
        public double PosY;
        public double Direction;
        public double Peace;
        double PI2 = Math.PI * 2.0;
        double CAM_ROT_SPEED = 0.2;
        double CAM_MOVE_SPEED = 0.5;

        public Player(double x, double y, double dir)
        {
            PosX = x;
            PosY = y;
            Direction = dir;
            Peace = 0;
        }

        public void Rotate(double angle)
        {
            Direction = (Direction + angle + PI2) % PI2;
        }

        public void Walk(double dist)
        {
            double dx = Math.Cos(Direction) * dist;
            double dy = Math.Sin(Direction) * dist;

            //if (m_map.Get(PosX + dx, PosY) <= 0)
            //    PosX += dx;

            //if (m_map.Get(PosX, PosY + dy) <= 0)
            //    PosY += dy;

            Peace += dist;
        }

        public void Update(double seconds)
        {
            double finalRotSpeed = seconds * CAM_ROT_SPEED;
            double finalMoveSpeed = seconds * CAM_MOVE_SPEED;
            // Check the most significant bit to see if the key is down

            short key = GetAsyncKeyState(0x1B);
            if ((key & 0x8000) > 0)
            {
                PostQuitMessage(0x0012);
            }

            key = GetAsyncKeyState(0x25);
            if ((key & 0x8000) > 0)
            {
                Rotate(-Math.PI * finalRotSpeed);
            }

            key = GetAsyncKeyState(0x27);
            // Check the most significant bit to see if the key is down
            if ((key & 0x8000) > 0)
            {
               Rotate(Math.PI * finalRotSpeed);
            }

            key = GetAsyncKeyState(0x26);
            // Check the most significant bit to see if the key is down
            if ((key & 0x8000) > 0)
            {
                Walk(3 * finalMoveSpeed);
            }

            key = GetAsyncKeyState(0x28);
            // Check the most significant bit to see if the key is down
            if ((key & 0x8000) > 0)
            {
                Walk(-3 * finalMoveSpeed);
            }
            
        }


        [DllImport("User32.dll")]
        private static extern short GetAsyncKeyState(System.Windows.Forms.Keys vKey);
        [DllImport("User32.dll")]
        private static extern short GetAsyncKeyState(System.Int32 vKey);
        [DllImport("user32.dll")]
        static extern void PostQuitMessage(int nExitCode);
    }
}
