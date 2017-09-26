using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace rayCasterTest
{
    public class Face
    {
        public int[] vIndex;
        public int[] nIndex;
        public int[] tIndex;

        public Vector3 normal = new Vector3(0, 0, 0);

        public Face(int v1, int v2, int v3, int n1, int n2, int n3, int t1, int t2, int t3)
        {
            vIndex = new int[3];
            nIndex = new int[3];
            tIndex = new int[3];

            vIndex[0] = v1;
            vIndex[1] = v2;
            vIndex[2] = v3;

            nIndex[0] = n1;
            nIndex[1] = n2;
            nIndex[2] = n3;

            tIndex[0] = t1;
            tIndex[1] = t2;
            tIndex[2] = t3;
  
        }

        public void setNormal(Vector3 v1)
        {
            normal.x = v1.x;
            normal.y = v1.y;
            normal.z = v1.z;
        }
    }
}
