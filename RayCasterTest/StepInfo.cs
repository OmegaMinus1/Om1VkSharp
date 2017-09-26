using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rayCasterTest
{
    public class StepInfo
    {
        public double x;
        public double y;
        public double height;
        public double distance;
        public double length2;
        public double shading;
        public double offset;
        internal double CellX;
        internal double CellY;
        internal bool primativeHit;
        internal bool hasPrimative;
        internal Vector3 primativeHitColor = new Vector3(0, 0, 0);

        public StepInfo(double X, double Y, double Height, double Dist, double Lengh2, double Shading, double Offset, bool HasPrimative)
        {
            x = X;
            y = Y;
            height = Height;
            distance = Dist;
            length2 = Lengh2;
            shading = Shading;
            offset = Offset;
            hasPrimative = HasPrimative;
          
        }
    }
}
