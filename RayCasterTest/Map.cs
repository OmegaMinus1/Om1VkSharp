using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rayCasterTest
{
    public class Map
    {
        public uint m_Size;
        public Cell[] m_Grid;
        public double m_fLight;

        public Map()
        {
        }

        public Map(uint Size)
        {
            m_Size = Size;
            m_fLight = 0;
            m_Grid = new Cell[Size * Size];
            for(int loop=0;loop< Size * Size;loop++)
            {
                m_Grid[loop] = new Cell();
            }
        }

        public int Get(double x, double y)
        {
            x = Math.Floor(x);
            y = Math.Floor(y);

            if (x < 0 || (x > (m_Size - 1)) || y < 0 || (y > (m_Size - 1)))
            {
                return -1;
            }

            return (int)m_Grid[(int)(y * m_Size + x)].value;
        }

        public void Randomize()
        {
            Random rnd = new Random();
            for (uint i = 0; i < m_Size * m_Size; ++i)
            {
                m_Grid[i].value = (uint)rnd.Next(0, 2);
            }
        }

        public Ray Cast(Map map, Location Loc, double Angle, double range)
        {
            StepInfo SI = new StepInfo(Loc.x, Loc.y, 0, 0, 0, 0, 0, false);
            
            Ray ray = new Ray(map, SI, Math.Sin(Angle), Math.Cos(Angle), range);
            
            return ray;
        }

        public void Update(double seconds)
        {
            //foreach (Cell cell in m_Grid)
            //{
            //    Model prim = cell.primative;

            //    if (prim != null)
            //    {
            //        prim.rotate(0, 0, 32 * seconds);
            //    }
            //}
        }

        internal int GetCellId(double x, double y)
        {
            x = Math.Floor(x);
            y = Math.Floor(y);

            if (x < 0 || (x > (m_Size - 1)) || y < 0 || (y > (m_Size - 1)))
            {
                return -1;
            }

            return (int)(y * m_Size + x);
        }
    }
}
