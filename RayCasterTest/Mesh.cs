using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;


namespace rayCasterTest
{
    public class Mesh
    {
        public List<Face> objFaces = new List<Face>();

        public List<string> objMaterials = new List<string>();

        public string name = string.Empty;

        //For Eye Only
        internal void traceRay(List<Vector3> objVertices, List<Vector3> objVerticesUV, List<Vector3> objVerticesNormals, Vector3 RayBegin, Vector3 RayEnd, Vector3 RayDir, ref bool hitTest, ref double hitDist, ref Vector3 hitPointTest, ref Vector3 hitNormalTest, ref Vector3 hitColorTest, ref int hitFaceIndex, ref double hitU, ref double hitV, ref Vector3 worldPos)
        {
            hitPointTest = new Vector3(0, 0, 0);
            hitNormalTest = new Vector3(0, 0, 0);
            hitColorTest = new Vector3(0, 0, 0);

            //Vector3[] POLYGON = new Vector3[3];

            double tempDistance = 100000.0f;
            int faceIndex = -1;
            Vector3 tempIntersectionPoint = new Vector3(0, 0, 0);
            Vector3 tempNormal = new Vector3(0, 0, 0);
            Vector3 tempColor = new Vector3(0, 0, 0);
            double tempU = 0.0f;
            double tempV = 0.0f;

            //STRUCT_RAY sr = new STRUCT_RAY();
            //IntPtr lParam = Marshal.AllocCoTaskMem(Marshal.SizeOf(sr));
            for (int faceloop = 0; faceloop < objFaces.Count; ++faceloop)
            {
                //STRUCT_RAY sr = new STRUCT_RAY();
                //sr.originX = (float)RayBegin.x;
                //sr.originY = (float)RayBegin.y;
                //sr.originZ = (float)RayBegin.z;

                //sr.directionX = (float)RayDir.x;
                //sr.directionY = (float)RayDir.y;
                //sr.directionZ = (float)RayDir.z;

                //sr.v1X = (float)(objVertices[objFaces[faceloop].vIndex[0]].x + worldPos.x);
                //sr.v1Y = (float)(objVertices[objFaces[faceloop].vIndex[0]].y + worldPos.y);
                //sr.v1Z = (float)(objVertices[objFaces[faceloop].vIndex[0]].z + worldPos.z);

                //sr.v2X = (float)(objVertices[objFaces[faceloop].vIndex[1]].x + worldPos.x);
                //sr.v2Y = (float)(objVertices[objFaces[faceloop].vIndex[1]].y + worldPos.y);
                //sr.v2Z = (float)(objVertices[objFaces[faceloop].vIndex[1]].z + worldPos.z);

                //sr.v3X = (float)(objVertices[objFaces[faceloop].vIndex[2]].x + worldPos.x);
                //sr.v3Y = (float)(objVertices[objFaces[faceloop].vIndex[2]].y + worldPos.y);
                //sr.v3Z = (float)(objVertices[objFaces[faceloop].vIndex[2]].z + worldPos.z);

                //sr.pDist = 0;

                //Marshal.StructureToPtr(sr, lParam, false);

                //float para_t = 0.0f;
                //int ret = IntersectRayTriangle(lParam, out para_t);


                Vector3 p1 = objVertices[objFaces[faceloop].vIndex[0]] + worldPos;

                Vector3 normal = objFaces[faceloop].normal;

                double dom = (normal.x * p1.x) + (normal.y * p1.y) + (normal.z * p1.z);

                double a_sign = (normal.x * RayBegin.x) + (normal.y * RayBegin.y) + (normal.z * RayBegin.z);

                double b_sign = (normal.x * RayEnd.x) + (normal.y * RayEnd.y) + (normal.z * RayEnd.z);

                if (a_sign + -dom <= 0 && b_sign + -dom >= 0)
                {
                    ////Ray is pierceing the face's plane
                    double para_t = (dom - a_sign) / (((normal.x * RayDir.x) + (normal.y * RayDir.y) + (normal.z * RayDir.z)) + vectorMath.EPSILON);

                    double intersectionPointx = RayBegin.x + (RayDir.x * para_t);
                    double intersectionPointy = RayBegin.y + (RayDir.y * para_t);
                    double intersectionPointz = RayBegin.z + (RayDir.z * para_t);

                    Vector3 p2 = objVertices[objFaces[faceloop].vIndex[1]] + worldPos;
                    Vector3 p3 = objVertices[objFaces[faceloop].vIndex[2]] + worldPos;

                    if (vectorMath.PointInTriangleVectors3(ref p1, p2, p3, ref intersectionPointx, ref intersectionPointy, ref intersectionPointz))
                    {
                        if ((para_t) < tempDistance)
                        {
                            tempDistance = (para_t);
                            faceIndex = faceloop;

                            Vector3 uv1 = objVerticesUV[objFaces[faceloop].tIndex[0]];
                            Vector3 uv2 = objVerticesUV[objFaces[faceloop].tIndex[1]];
                            Vector3 uv3 = objVerticesUV[objFaces[faceloop].tIndex[2]];

                            Vector3 n1 = objVerticesNormals[objFaces[faceloop].nIndex[0]];
                            Vector3 n2 = objVerticesNormals[objFaces[faceloop].nIndex[1]];
                            Vector3 n3 = objVerticesNormals[objFaces[faceloop].nIndex[2]];

                            Vector3 f = new Vector3(intersectionPointx, intersectionPointy, intersectionPointz);

                            Vector3 f1 = p1 - f;
                            Vector3 f2 = p2 - f;
                            Vector3 f3 = p3 - f;

                            // calculate the areas and factors (order of parameters doesn't matter):
                            double a = vectorMath.cross(p1 - p2, p1 - p3).magnitude(); // main triangle area a
                            double a1 = vectorMath.cross(f2, f3).magnitude() / a; // p1's triangle area / a
                            double a2 = vectorMath.cross(f3, f1).magnitude() / a; // p2's triangle area / a 
                            double a3 = vectorMath.cross(f1, f2).magnitude() / a; // p3's triangle area / a

                            // find the uv corresponding to point f (uv1/uv2/uv3 are associated to p1/p2/p3):
                            Vector3 uv = uv1 * a1 + uv2 * a2 + uv3 * a3;

                            Vector3 vertexNormal = n1 * a1 + n2 * a2 + n3 * a3;

                            tempU = uv.x;
                            tempV = uv.y;

                            tempIntersectionPoint.x = intersectionPointx;
                            tempIntersectionPoint.y = intersectionPointy;
                            tempIntersectionPoint.z = intersectionPointz;
                            tempNormal = vertexNormal;
                            tempColor.x = 1.0f;
                            tempColor.y = 1.0f;
                            tempColor.z = 1.0f;
                        }
                    }
                }
            }

            if (faceIndex > -1)
            {
                //We  have the closest point in the mesh
                hitTest = true;
                hitDist = tempDistance;

                hitPointTest = tempIntersectionPoint;
                hitNormalTest = tempNormal;
                hitColorTest = tempColor;

                hitFaceIndex = faceIndex;

                hitU = tempU;
                hitV = tempV;
            }
            else
            {
                hitTest = false;
                hitDist = 100000.0f;
                hitFaceIndex = faceIndex;
            }

        }


        //internal void traceRay(List<Vector3> objVertices, List<Vector3> objVerticesUV, List<Vector3> objVerticesNormals, Vector3 RayBegin, Vector3 RayEnd, Vector3 RayDir, ref bool hitTest, ref double hitDist, ref Vector3 hitPointTest, ref Vector3 hitNormalTest, ref Vector3 hitColorTest, ref int hitFaceIndex, ref double hitU, ref double hitV, ref Vector3 worldPos)
        //{
        //    hitPointTest = new Vector3(0, 0, 0);
        //    hitNormalTest = new Vector3(0, 0, 0);
        //    hitColorTest = new Vector3(0, 0, 0);

        //    //Vector3[] POLYGON = new Vector3[3];

        //    double tempDistance = 100000.0f;
        //    int faceIndex = -1;
        //    Vector3 tempIntersectionPoint = new Vector3(0, 0, 0);
        //    Vector3 tempNormal = new Vector3(0, 0, 0);
        //    Vector3 tempColor = new Vector3(0, 0, 0);
        //    double tempU = 0.0f;
        //    double tempV = 0.0f;

        //    for (int faceloop = 0; faceloop < objFaces.Count; ++faceloop)
        //    {
        //        Vector3 p1 = objVertices[objFaces[faceloop].vIndex[0]] + worldPos;

        //        Vector3 normal = objFaces[faceloop].normal;

        //        double dom = (normal.x * p1.x) + (normal.y * p1.y) + (normal.z * p1.z);

        //        double a_sign = (normal.x * RayBegin.x) + (normal.y * RayBegin.y) + (normal.z * RayBegin.z);

        //        double b_sign = (normal.x * RayEnd.x) + (normal.y * RayEnd.y) + (normal.z * RayEnd.z);

        //        if (a_sign + -dom <= 0 && b_sign + -dom >= 0)
        //        {
        //            //Ray is pierceing the face's plane
        //            double para_t = (dom - a_sign) / (((normal.x * RayDir.x) + (normal.y * RayDir.y) + (normal.z * RayDir.z)) + vectorMath.EPSILON);

        //            double intersectionPointx = RayBegin.x + (RayDir.x * para_t);
        //            double intersectionPointy = RayBegin.y + (RayDir.y * para_t);
        //            double intersectionPointz = RayBegin.z + (RayDir.z * para_t);

        //            Vector3 p2 = objVertices[objFaces[faceloop].vIndex[1]] + worldPos;
        //            Vector3 p3 = objVertices[objFaces[faceloop].vIndex[2]] + worldPos;

        //            if (vectorMath.PointInTriangleVectors3(ref p1, p2, p3, ref intersectionPointx, ref intersectionPointy, ref intersectionPointz))
        //            {
        //                if (Math.Abs(para_t) < tempDistance)
        //                {
        //                    tempDistance = Math.Abs(para_t);
        //                    faceIndex = faceloop;

        //                    Vector3 uv1 = objVerticesUV[objFaces[faceloop].tIndex[0]];
        //                    Vector3 uv2 = objVerticesUV[objFaces[faceloop].tIndex[1]];
        //                    Vector3 uv3 = objVerticesUV[objFaces[faceloop].tIndex[2]];

        //                    Vector3 n1 = objVerticesNormals[objFaces[faceloop].nIndex[0]];
        //                    Vector3 n2 = objVerticesNormals[objFaces[faceloop].nIndex[1]];
        //                    Vector3 n3 = objVerticesNormals[objFaces[faceloop].nIndex[2]];

        //                    Vector3 f = new Vector3(intersectionPointx, intersectionPointy, intersectionPointz);

        //                    Vector3 f1 = p1 - f;
        //                    Vector3 f2 = p2 - f;
        //                    Vector3 f3 = p3 - f;

        //                    // calculate the areas and factors (order of parameters doesn't matter):
        //                    double a = vectorMath.cross(p1 - p2, p1 - p3).magnitude(); // main triangle area a
        //                    double a1 = vectorMath.cross(f2, f3).magnitude() / a; // p1's triangle area / a
        //                    double a2 = vectorMath.cross(f3, f1).magnitude() / a; // p2's triangle area / a 
        //                    double a3 = vectorMath.cross(f1, f2).magnitude() / a; // p3's triangle area / a

        //                    // find the uv corresponding to point f (uv1/uv2/uv3 are associated to p1/p2/p3):
        //                    Vector3 uv = uv1 * a1 + uv2 * a2 + uv3 * a3;

        //                    Vector3 vertexNormal = n1 * a1 + n2 * a2 + n3 * a3;

        //                    tempU = uv.x;
        //                    tempV = uv.y;

        //                    tempIntersectionPoint.x = intersectionPointx;
        //                    tempIntersectionPoint.y = intersectionPointy;
        //                    tempIntersectionPoint.z = intersectionPointz;
        //                    tempNormal = vertexNormal;
        //                    tempColor.x = 1.0f;
        //                    tempColor.y = 1.0f;
        //                    tempColor.z = 1.0f;
        //                }
        //            }
        //        }
        //    }

        //    if (faceIndex > -1)
        //    {
        //        //We  have the closest point in the mesh
        //        hitTest = true;
        //        hitDist = tempDistance;

        //        hitPointTest = tempIntersectionPoint;
        //        hitNormalTest = tempNormal;
        //        hitColorTest = tempColor;

        //        hitFaceIndex = faceIndex;

        //        hitU = tempU;
        //        hitV = tempV;
        //    }
        //    else
        //    {
        //        hitTest = false;
        //        hitDist = 100000.0f;
        //        hitFaceIndex = faceIndex;
        //    }

        //}

        //For lights only
        internal void traceRayHitTest(List<Vector3> objVertices, Vector3 RayBegin, Vector3 RayEnd, Vector3 RayDir, ref bool hitTest, ref double hitDist)
        {
            //Vector3[] POLYGON = new Vector3[3];

            double tempDistance = 100000.0f;
            int faceIndex = -1;

            //Vector3 intersectionPoint = new Vector3(0, 0, 0); 

            for (int faceloop = 0; faceloop < objFaces.Count; ++faceloop)
            //Parallel.For(0, objFaces.Count, (faceloop) =>
            {


                Vector3 normal = objFaces[faceloop].normal;

                Vector3 p1 = objVertices[objFaces[faceloop].vIndex[0]];

                //double dom = vectorMath.dot3(normal.x, normal.y, normal.z, p1.x, p1.y, p1.z);
                double dom = (normal.x * p1.x) + (normal.y * p1.y) + (normal.z * p1.z);

                //double a_sign = vectorMath.dot3(normal.x, normal.y, normal.z, RayBegin.x, RayBegin.y, RayBegin.z);
                double a_sign = (normal.x * RayBegin.x) + (normal.y * RayBegin.y) + (normal.z * RayBegin.z);

                //double b_sign = vectorMath.dot3(normal.x, normal.y, normal.z, RayEnd.x, RayEnd.y, RayEnd.z);
                double b_sign = (normal.x * RayEnd.x) + (normal.y * RayEnd.y) + (normal.z * RayEnd.z);

                //double dom = (normal * p1);

                //double a_sign = (normal * RayBegin);

                //double b_sign = (normal * RayEnd);

                if (a_sign + -dom <= 0 && b_sign + -dom >= 0)
                {
                    //Ray is pierceing the face

                    //double para_t = (dom - a_sign) / (vectorMath.EPSILON + (normal * RayDir));
                    double para_t = (dom - a_sign) / (((normal.x * RayDir.x) + (normal.y * RayDir.y) + (normal.z * RayDir.z)) + vectorMath.EPSILON);

                    //Vector3 intersectionPoint = new Vector3(0, 0, 0); //RayBegin + RayDir * para_t;
                    double intersectionPointx = RayBegin.x + (RayDir.x * para_t);
                    double intersectionPointy = RayBegin.y + (RayDir.y * para_t);
                    double intersectionPointz = RayBegin.z + (RayDir.z * para_t);

                    if (vectorMath.PointInTriangleVectors3(ref p1, objVertices[objFaces[faceloop].vIndex[1]], objVertices[objFaces[faceloop].vIndex[2]], ref intersectionPointx, ref intersectionPointy, ref intersectionPointz))
                    {
                        //point_inside = true;

                        //double distanceToIntersection = Math.Abs(Math.Sqrt((intersectionPointx - RayBegin.x) * (intersectionPointx - RayBegin.x) +
                        //                                       (intersectionPointy - RayBegin.y) * (intersectionPointy - RayBegin.y) +
                        //                                       (intersectionPointz - RayBegin.z) * (intersectionPointz - RayBegin.z))); //vectorMath.distance(RayBegin, intersectionPoint);

                        if (Math.Abs(para_t) < tempDistance)
                        //if (distanceToIntersection < tempDistance)
                        {
                            tempDistance = Math.Abs(para_t);
                            faceIndex = faceloop;
                            break;
                        }
                    }
                }
            }


            if (faceIndex > -1)
            {
                //We  have the closest point in the mesh
                hitTest = true;
                hitDist = tempDistance;

            }
            else
            {
                hitTest = false;
                hitDist = 100000.0f;

            }

        }

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("Om1RayLib.dll", EntryPoint = "?IntersectRayTriangle@COm1RayLib@@SAHPEAHPEAM@Z", CharSet = CharSet.Auto)]
        public static extern int IntersectRayTriangle(IntPtr Ray, out float dist);

        [StructLayout(LayoutKind.Sequential)]
        private struct STRUCT_RAY
        {
            public float originX;
            public float originY;
            public float originZ;

            public float directionX;
            public float directionY;
            public float directionZ;

            public float v1X;
            public float v1Y;
            public float v1Z;

            public float v2X;
            public float v2Y;
            public float v2Z;

            public float v3X;
            public float v3Y;
            public float v3Z;

            public float pDist;
        }

    }
}
