using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShaderGenesis
{
    public class Model
    {

        public const double pi180ed = Math.PI / 180.0;
        public List<Mesh> objMeshes = new List<Mesh>();

        public List<Vector3> objVertices = new List<Vector3>();
        public List<Vector3> objVerticesNormals = new List<Vector3>();
        public List<Vector3> objVerticesUV = new List<Vector3>();

        string name = string.Empty;

        public Vector3 worldPos = new Vector3(0, 0, 0);

        public void loadObj(string filenameAndPath)
        {
            List<string> objLines = new List<string>();
            List<string> materialLines = new List<string>();

            objLines.ReadLines(filenameAndPath);

            string materialFile = string.Empty;

            string meshName = string.Empty;
            string meshMeterial = string.Empty;
            Mesh mesh = new Mesh();
            bool facesAdded = false;

            for (int loop = 0; loop < objLines.Count; ++loop)
            {
                string line = objLines[loop];

                if (line.Trim() == string.Empty)
                {
                    continue;
                }
                else if (line.Trim() == "#")
                {
                    continue;
                }
                else if (line.IndexOf("mtllib") == 0)
                {
                    materialFile = line.Replace("mtllib", string.Empty).Trim();
                }
                else if (line.IndexOf("# object") == 0)
                {
                    name = line.Replace("# object", string.Empty).Trim();
                }
                else if (line.IndexOf("#") == 0 && line.IndexOf("vertices") > -1)
                {
                    //vertices total count
                    continue;
                }
                else if (line.IndexOf("#") == 0 && line.IndexOf("vertex normals") > -1)
                {
                    //vertex normals total count
                    continue;
                }
                else if (line.IndexOf("#") == 0 && line.IndexOf("texture coords") > -1)
                {
                    //texture coords total count
                    continue;
                }
                else if (line.IndexOf("#") == 0 && line.IndexOf("faces") > -1)
                {
                    //faces total count
                    continue;
                }
                else if (line.IndexOf("vn") == 0)
                {
                    //vertex normals
                    //vn -0.8829 0.1455 0.4465
                    string temp = line.Replace("vn", string.Empty).Trim();
                    string[] parts = temp.Split(' ');

                    Vector3 tempNormal = new Vector3(double.Parse(parts[0]), double.Parse(parts[1]), double.Parse(parts[2]));

                    objVerticesNormals.Add(tempNormal);
                }
                else if (line.IndexOf("vt") == 0)
                {
                    //texture coords
                    //vt 0.2077 0.8234 0.0000
                    string temp = line.Replace("vt", string.Empty).Trim();
                    string[] parts = temp.Split(' ');

                    Vector3 tempUV = new Vector3(double.Parse(parts[0]), double.Parse(parts[1]), double.Parse(parts[2]));

                    objVerticesUV.Add(tempUV);
                }
                else if (line.IndexOf("v") == 0)
                {
                    //vertices
                    //v  -64.0564 110.0555 -53.6867
                    string temp = line.Replace("v", string.Empty).Trim();
                    string[] parts = temp.Split(' ');

                    Vector3 tempVertices = new Vector3(double.Parse(parts[0]), double.Parse(parts[1]), double.Parse(parts[2]));

                    objVertices.Add(tempVertices);

                }
                else if (line.IndexOf("g") == 0)
                {
                    mesh.name = line.Replace("g", string.Empty).Trim();

                    if (facesAdded == true)
                    {
                        objMeshes.Add(mesh);

                        facesAdded = false;

                        mesh = new Mesh();
                    }

                }
                else if (line.IndexOf("usemtl") == 0)
                {
                    meshMeterial = line.Replace("usemtl", string.Empty).Trim();
                }
                else if (line.IndexOf("s") == 0)
                {
                    //smoothing group
                    continue;
                }
                else if (line.IndexOf("f") == 0)
                {
                    //face
                    string temp = line.Replace("f", string.Empty).Trim();
                    string[] parts = temp.Split(' ');

                    //Face1
                    string[] partsA = parts[0].Split('/');
                    string[] partsB = parts[1].Split('/');
                    string[] partsC = parts[2].Split('/');

                    Face face = new Face(int.Parse(partsA[0]) - 1, int.Parse(partsB[0]) - 1, int.Parse(partsC[0]) - 1,
                                         int.Parse(partsA[2]) - 1, int.Parse(partsB[2]) - 1, int.Parse(partsC[2]) - 1,
                                         int.Parse(partsA[1]) - 1, int.Parse(partsB[1]) - 1, int.Parse(partsC[1]) - 1);

                    Vector3 v1 = new Vector3(objVertices[face.vIndex[0]].x, objVertices[face.vIndex[0]].y, objVertices[face.vIndex[0]].z);
                    Vector3 v2 = new Vector3(objVertices[face.vIndex[1]].x, objVertices[face.vIndex[1]].y, objVertices[face.vIndex[1]].z);
                    Vector3 v3 = new Vector3(objVertices[face.vIndex[2]].x, objVertices[face.vIndex[2]].y, objVertices[face.vIndex[2]].z);

                    Vector3 vNormal = new Vector3(0, 0, 0);
                    vectorMath.calcnormal(v1, v2, v3, ref vNormal);

                    if (!(vNormal.x == 0.0f && vNormal.y == 0.0f && vNormal.z == 0.0f))
                    {
                        face.setNormal(vNormal);

                        mesh.objFaces.Add(face);

                        facesAdded = true;
                    }


                }


            }

            if (facesAdded == true)
            {
                objMeshes.Add(mesh);
            }

            objLines.Clear();

            //Load Material file if exists

        }

        internal void traceRay(Vector3 RayBegin, Vector3 RayEnd, ref bool hit, ref double hitDistance, ref Vector3 hitPoint, ref Vector3 hitNormal, ref Vector3 hitColor, ref int hitFaceIndex, ref int hitMeshIndex, ref double hitFaceU, ref double hitFaceV)
        {
            hitPoint = new Vector3(0, 0, 0);
            hitNormal = new Vector3(0, 0, 0);
            hitColor = new Vector3(0, 0, 0);

            //Ray caclulated for eye
            Vector3 RayDir = new Vector3(RayEnd.x - RayBegin.x, RayEnd.y - RayBegin.y, RayEnd.z - RayBegin.z);

            RayDir.normalize();

            double objDistance = 100000.0f;
            int objIndex = -1;
            int tempFaceIndex = -1;

            Vector3 objPoint = new Vector3(0.0f, 0.0f, 0.0f);
            Vector3 objNormal = new Vector3(0.0f, 0.0f, 0.0f);
            Vector3 objColor = new Vector3(0.0f, 0.0f, 0.0f);
            double objU = 0.0f;
            double objV = 0.0f;

            Vector3 hitPointTest = new Vector3(0.0f, 0.0f, 0.0f);
            Vector3 hitNormalTest = new Vector3(0.0f, 0.0f, 0.0f);
            Vector3 hitColorTest = new Vector3(0.0f, 0.0f, 0.0f);

            int faceIndex = -1;

            for (int meshloop = 0; meshloop < objMeshes.Count; ++meshloop)
            {
                bool hitTest = false;
                double hitDist = 0.0f;
                double hitU = 0.0f;
                double hitV = 0.0f;

                objMeshes[meshloop].traceRay(objVertices, objVerticesUV, objVerticesNormals, RayBegin, RayEnd, RayDir, ref hitTest, ref hitDist, ref hitPointTest, ref hitNormalTest, ref hitColorTest, ref faceIndex, ref hitU, ref hitV, ref worldPos);

                if (hitTest == true && hitDist < objDistance)
                {
                    objDistance = hitDist;
                    objIndex = meshloop;

                    objPoint.x = hitPointTest.x;
                    objPoint.y = hitPointTest.y;
                    objPoint.z = hitPointTest.z;

                    objNormal.x = hitNormalTest.x;
                    objNormal.y = hitNormalTest.y;
                    objNormal.z = hitNormalTest.z;

                    objColor.x = hitColorTest.x;
                    objColor.y = hitColorTest.y;
                    objColor.z = hitColorTest.z;

                    objU = hitU;
                    objV = hitV;

                    tempFaceIndex = faceIndex;
                }

            }

            if (objIndex > -1)
            {
                //We have the closest point
                hit = true;
                hitDistance = objDistance;

                hitPoint.x = objPoint.x;
                hitPoint.y = objPoint.y;
                hitPoint.z = objPoint.z;

                hitNormal.x = objNormal.x;
                hitNormal.y = objNormal.y;
                hitNormal.z = objNormal.z;

                hitColor.x = objColor.x;
                hitColor.y = objColor.y;
                hitColor.z = objColor.z;

                hitFaceIndex = tempFaceIndex;

                hitMeshIndex = objIndex;

                hitFaceU = objU;
                hitFaceV = objV;
            }
            else
            {
                hit = false;
                hitDistance = 100000.0f;

                hitFaceIndex = tempFaceIndex;

                hitMeshIndex = objIndex;

            }

        }

        public void rotate(double xan, double yan, double zan)
        {
            int product = 0;

            double[,] rotate_x = new double[4, 4];
            double[,] rotate_y = new double[4, 4];
            double[,] rotate_z = new double[4, 4];

            double[,] rotate = new double[4, 4];
            double[,] temp = new double[4, 4];

            //double temp_x, temp_y, temp_z;

            if (xan == 0 && yan == 0 && zan == 0) return;

            Array.Clear(rotate, 0, 16);
            //memset(rotate, 0, 4 * 16);

            rotate[0, 0] = rotate[1, 1] = rotate[2, 2] = rotate[3, 3] = 1;

            if (xan > 0.0f)
            {
                Array.Clear(rotate_x, 0, 16);
                //memset(rotate_x, 0, 4 * 16);
                rotate_x[0, 0] = rotate_x[1, 1] = rotate_x[2, 2] = rotate_x[3, 3] = 1;

                rotate_x[1, 1] = (Math.Cos(xan * pi180ed));
                rotate_x[1, 2] = (Math.Sin(xan * pi180ed));
                rotate_x[2, 1] = (-Math.Sin(xan * pi180ed));
                rotate_x[2, 2] = (Math.Cos(xan * pi180ed));
                product |= 4;
            }
            if (yan > 0.0f)
            {
                Array.Clear(rotate_y, 0, 16);
                //memset(rotate_y, 0, 4 * 16);
                rotate_y[0,0] = rotate_y[1,1] = rotate_y[2,2] = rotate_y[3,3] = 1;

                rotate_y[0, 0] = (Math.Cos(yan * pi180ed));
                rotate_y[0, 2] = (-Math.Sin(yan * pi180ed));
                rotate_y[2, 0] = (Math.Sin(yan * pi180ed));
                rotate_y[2, 2] = (Math.Cos(yan * pi180ed));
                product |= 2;
            }
            if (zan > 0.0f)
            {
                Array.Clear(rotate_z, 0, 16);
                //memset(rotate_z, 0, 4 * 16);
                rotate_z[0,0] = rotate_z[1,1] = rotate_z[2,2] = rotate_z[3,3] = 1;

                rotate_z[0, 0] = (Math.Cos(zan * pi180ed));
                rotate_z[0, 1] = (Math.Sin(zan * pi180ed));
                rotate_z[1, 0] = (-Math.Sin(zan * pi180ed));
                rotate_z[1, 1] = (Math.Cos(zan * pi180ed));
                product |= 1;
            }

            switch (product)
            {
                case 1:
                    {
                        for (int loop1 = 0; loop1 < 4; loop1++)
                            for (int loop2 = 0; loop2 < 4; loop2++)
                                rotate[loop1, loop2] = rotate_z[loop1, loop2];
                    }
                    break;

                case 2:
                    {
                        for (int loop1 = 0; loop1 < 4; loop1++)
                            for (int loop2 = 0; loop2 < 4; loop2++)
                                rotate[loop1, loop2] = rotate_y[loop1, loop2];
                    }
                    break;

                case 3:
                    {
                        rotate[0, 0] = rotate_y[0, 0] * rotate_z[0, 0] + rotate_y[0, 1] * rotate_z[1, 0] + rotate_y[0, 2] * rotate_z[2, 0];
                        rotate[0, 1] = rotate_y[0, 0] * rotate_z[0, 1] + rotate_y[0, 1] * rotate_z[1, 1] + rotate_y[0, 2] * rotate_z[2, 1];
                        rotate[0, 2] = rotate_y[0, 0] * rotate_z[0, 2] + rotate_y[0, 1] * rotate_z[1, 2] + rotate_y[0, 2] * rotate_z[2, 2];
                        rotate[0, 3] = 0;

                        rotate[1, 0] = rotate_y[1, 0] * rotate_z[0, 0] + rotate_y[1, 1] * rotate_z[1, 0] + rotate_y[1, 2] * rotate_z[2, 0];
                        rotate[1, 1] = rotate_y[1, 0] * rotate_z[0, 1] + rotate_y[1, 1] * rotate_z[1, 1] + rotate_y[1, 2] * rotate_z[2, 1];
                        rotate[1, 2] = rotate_y[1, 0] * rotate_z[0, 2] + rotate_y[1, 1] * rotate_z[1, 2] + rotate_y[1, 2] * rotate_z[2, 2];
                        rotate[1, 3] = 0;

                        rotate[2, 0] = rotate_y[2, 0] * rotate_z[0, 0] + rotate_y[2, 1] * rotate_z[1, 0] + rotate_y[2, 2] * rotate_z[2, 0];
                        rotate[2, 1] = rotate_y[2, 0] * rotate_z[0, 1] + rotate_y[2, 1] * rotate_z[1, 1] + rotate_y[2, 2] * rotate_z[2, 1];
                        rotate[2, 2] = rotate_y[2, 0] * rotate_z[0, 2] + rotate_y[2, 1] * rotate_z[1, 2] + rotate_y[2, 2] * rotate_z[2, 2];
                        rotate[2, 3] = 0;

                        rotate[3, 0] = rotate_y[3, 0] * rotate_z[0, 0] + rotate_y[3, 1] * rotate_z[1, 0] + rotate_y[3, 2] * rotate_z[2, 0] + rotate_z[3, 0];
                        rotate[3, 1] = rotate_y[3, 0] * rotate_z[0, 1] + rotate_y[3, 1] * rotate_z[1, 1] + rotate_y[3, 2] * rotate_z[2, 1] + rotate_z[3, 1];
                        rotate[3, 2] = rotate_y[3, 0] * rotate_z[0, 2] + rotate_y[3, 1] * rotate_z[1, 2] + rotate_y[3, 2] * rotate_z[2, 2] + rotate_z[3, 2];
                        rotate[3, 3] = 0;
                    }
                    break;

                case 4:
                    {
                        for (int loop1 = 0; loop1 < 4; loop1++)
                            for (int loop2 = 0; loop2 < 4; loop2++)
                                rotate[loop1, loop2] = rotate_x[loop1, loop2];
                    }
                    break;

                case 5:
                    {
                        rotate[0, 0] = rotate_x[0, 0] * rotate_z[0, 0] + rotate_x[0, 1] * rotate_z[1, 0] + rotate_x[0, 2] * rotate_z[2, 0];
                        rotate[0, 1] = rotate_x[0, 0] * rotate_z[0, 1] + rotate_x[0, 1] * rotate_z[1, 1] + rotate_x[0, 2] * rotate_z[2, 1];
                        rotate[0, 2] = rotate_x[0, 0] * rotate_z[0, 2] + rotate_x[0, 1] * rotate_z[1, 2] + rotate_x[0, 2] * rotate_z[2, 2];
                        rotate[0, 3] = 0;

                        rotate[1, 0] = rotate_x[1, 0] * rotate_z[0, 0] + rotate_x[1, 1] * rotate_z[1, 0] + rotate_x[1, 2] * rotate_z[2, 0];
                        rotate[1, 1] = rotate_x[1, 0] * rotate_z[0, 1] + rotate_x[1, 1] * rotate_z[1, 1] + rotate_x[1, 2] * rotate_z[2, 1];
                        rotate[1, 2] = rotate_x[1, 0] * rotate_z[0, 2] + rotate_x[1, 1] * rotate_z[1, 2] + rotate_x[1, 2] * rotate_z[2, 2];
                        rotate[1, 3] = 0;

                        rotate[2, 0] = rotate_x[2, 0] * rotate_z[0, 0] + rotate_x[2, 1] * rotate_z[1, 0] + rotate_x[2, 2] * rotate_z[2, 0];
                        rotate[2, 1] = rotate_x[2, 0] * rotate_z[0, 1] + rotate_x[2, 1] * rotate_z[1, 1] + rotate_x[2, 2] * rotate_z[2, 1];
                        rotate[2, 2] = rotate_x[2, 0] * rotate_z[0, 2] + rotate_x[2, 1] * rotate_z[1, 2] + rotate_x[2, 2] * rotate_z[2, 2];
                        rotate[2, 3] = 0;

                        rotate[3, 0] = rotate_x[3, 0] * rotate_z[0, 0] + rotate_x[3, 1] * rotate_z[1, 0] + rotate_x[3, 2] * rotate_z[2, 0] + rotate_z[3, 0];
                        rotate[3, 1] = rotate_x[3, 0] * rotate_z[0, 1] + rotate_x[3, 1] * rotate_z[1, 1] + rotate_x[3, 2] * rotate_z[2, 1] + rotate_z[3, 1];
                        rotate[3, 2] = rotate_x[3, 0] * rotate_z[0, 2] + rotate_x[3, 1] * rotate_z[1, 2] + rotate_x[3, 2] * rotate_z[2, 2] + rotate_z[3, 2];
                        rotate[3, 3] = 0;
                    }
                    break;

                case 6:
                    {
                        rotate[0, 0] = rotate_x[0, 0] * rotate_y[0, 0] + rotate_x[0, 1] * rotate_y[1, 0] + rotate_x[0, 2] * rotate_y[2, 0];
                        rotate[0, 1] = rotate_x[0, 0] * rotate_y[0, 1] + rotate_x[0, 1] * rotate_y[1, 1] + rotate_x[0, 2] * rotate_y[2, 1];
                        rotate[0, 2] = rotate_x[0, 0] * rotate_y[0, 2] + rotate_x[0, 1] * rotate_y[1, 2] + rotate_x[0, 2] * rotate_y[2, 2];
                        rotate[0, 3] = 0;

                        rotate[1, 0] = rotate_x[1, 0] * rotate_y[0, 0] + rotate_x[1, 1] * rotate_y[1, 0] + rotate_x[1, 2] * rotate_y[2, 0];
                        rotate[1, 1] = rotate_x[1, 0] * rotate_y[0, 1] + rotate_x[1, 1] * rotate_y[1, 1] + rotate_x[1, 2] * rotate_y[2, 1];
                        rotate[1, 2] = rotate_x[1, 0] * rotate_y[0, 2] + rotate_x[1, 1] * rotate_y[1, 2] + rotate_x[1, 2] * rotate_y[2, 2];
                        rotate[1, 3] = 0;

                        rotate[2, 0] = rotate_x[2, 0] * rotate_y[0, 0] + rotate_x[2, 1] * rotate_y[1, 0] + rotate_x[2, 2] * rotate_y[2, 0];
                        rotate[2, 1] = rotate_x[2, 0] * rotate_y[0, 1] + rotate_x[2, 1] * rotate_y[1, 1] + rotate_x[2, 2] * rotate_y[2, 1];
                        rotate[2, 2] = rotate_x[2, 0] * rotate_y[0, 2] + rotate_x[2, 1] * rotate_y[1, 2] + rotate_x[2, 2] * rotate_y[2, 2];
                        rotate[2, 3] = 0;

                        rotate[3, 0] = rotate_x[3, 0] * rotate_y[0, 0] + rotate_x[3, 1] * rotate_y[1, 0] + rotate_x[3, 2] * rotate_y[2, 0] + rotate_y[3, 0];
                        rotate[3, 1] = rotate_x[3, 0] * rotate_y[0, 1] + rotate_x[3, 1] * rotate_y[1, 1] + rotate_x[3, 2] * rotate_y[2, 1] + rotate_y[3, 1];
                        rotate[3, 2] = rotate_x[3, 0] * rotate_y[0, 2] + rotate_x[3, 1] * rotate_y[1, 2] + rotate_x[3, 2] * rotate_y[2, 2] + rotate_y[3, 2];
                        rotate[3, 3] = 0;
                    }
                    break;

                case 7:
                    {
                        temp[0, 0] = rotate_x[0, 0] * rotate_y[0, 0] + rotate_x[0, 1] * rotate_y[1, 0] + rotate_x[0, 2] * rotate_y[2, 0];
                        temp[0, 1] = rotate_x[0, 0] * rotate_y[0, 1] + rotate_x[0, 1] * rotate_y[1, 1] + rotate_x[0, 2] * rotate_y[2, 1];
                        temp[0, 2] = rotate_x[0, 0] * rotate_y[0, 2] + rotate_x[0, 1] * rotate_y[1, 2] + rotate_x[0, 2] * rotate_y[2, 2];
                        temp[0, 3] = 0;

                        temp[1, 0] = rotate_x[1, 0] * rotate_y[0, 0] + rotate_x[1, 1] * rotate_y[1, 0] + rotate_x[1, 2] * rotate_y[2, 0];
                        temp[1, 1] = rotate_x[1, 0] * rotate_y[0, 1] + rotate_x[1, 1] * rotate_y[1, 1] + rotate_x[1, 2] * rotate_y[2, 1];
                        temp[1, 2] = rotate_x[1, 0] * rotate_y[0, 2] + rotate_x[1, 1] * rotate_y[1, 2] + rotate_x[1, 2] * rotate_y[2, 2];
                        temp[1, 3] = 0;

                        temp[2, 0] = rotate_x[2, 0] * rotate_y[0, 0] + rotate_x[2, 1] * rotate_y[1, 0] + rotate_x[2, 2] * rotate_y[2, 0];
                        temp[2, 1] = rotate_x[2, 0] * rotate_y[0, 1] + rotate_x[2, 1] * rotate_y[1, 1] + rotate_x[2, 2] * rotate_y[2, 1];
                        temp[2, 2] = rotate_x[2, 0] * rotate_y[0, 2] + rotate_x[2, 1] * rotate_y[1, 2] + rotate_x[2, 2] * rotate_y[2, 2];
                        temp[2, 3] = 0;

                        temp[3, 0] = rotate_x[3, 0] * rotate_y[0, 0] + rotate_x[3, 1] * rotate_y[1, 0] + rotate_x[3, 2] * rotate_y[2, 0] + rotate_y[3, 0];
                        temp[3, 1] = rotate_x[3, 0] * rotate_y[0, 1] + rotate_x[3, 1] * rotate_y[1, 1] + rotate_x[3, 2] * rotate_y[2, 1] + rotate_y[3, 1];
                        temp[3, 2] = rotate_x[3, 0] * rotate_y[0, 2] + rotate_x[3, 1] * rotate_y[1, 2] + rotate_x[3, 2] * rotate_y[2, 2] + rotate_y[3, 2];
                        temp[3, 3] = 0;

                        rotate[0, 0] = temp[0, 0] * rotate_z[0, 0] + temp[0, 1] * rotate_z[1, 0] + temp[0, 2] * rotate_z[2, 0];
                        rotate[0, 1] = temp[0, 0] * rotate_z[0, 1] + temp[0, 1] * rotate_z[1, 1] + temp[0, 2] * rotate_z[2, 1];
                        rotate[0, 2] = temp[0, 0] * rotate_z[0, 2] + temp[0, 1] * rotate_z[1, 2] + temp[0, 2] * rotate_z[2, 2];
                        rotate[0, 3] = 0;

                        rotate[1, 0] = temp[1, 0] * rotate_z[0, 0] + temp[1, 1] * rotate_z[1, 0] + temp[1, 2] * rotate_z[2, 0];
                        rotate[1, 1] = temp[1, 0] * rotate_z[0, 1] + temp[1, 1] * rotate_z[1, 1] + temp[1, 2] * rotate_z[2, 1];
                        rotate[1, 2] = temp[1, 0] * rotate_z[0, 2] + temp[1, 1] * rotate_z[1, 2] + temp[1, 2] * rotate_z[2, 2];
                        rotate[1, 3] = 0;

                        rotate[2, 0] = temp[2, 0] * rotate_z[0, 0] + temp[2, 1] * rotate_z[1, 0] + temp[2, 2] * rotate_z[2, 0];
                        rotate[2, 1] = temp[2, 0] * rotate_z[0, 1] + temp[2, 1] * rotate_z[1, 1] + temp[2, 2] * rotate_z[2, 1];
                        rotate[2, 2] = temp[2, 0] * rotate_z[0, 2] + temp[2, 1] * rotate_z[1, 2] + temp[2, 2] * rotate_z[2, 2];
                        rotate[2, 3] = 0;

                        rotate[3, 0] = temp[3, 0] * rotate_z[0, 0] + temp[3, 1] * rotate_z[1, 0] + temp[3, 2] * rotate_z[2, 0] + rotate_z[3, 0];
                        rotate[3, 1] = temp[3, 0] * rotate_z[0, 1] + temp[3, 1] * rotate_z[1, 1] + temp[3, 2] * rotate_z[2, 1] + rotate_z[3, 1];
                        rotate[3, 2] = temp[3, 0] * rotate_z[0, 2] + temp[3, 1] * rotate_z[1, 2] + temp[3, 2] * rotate_z[2, 2] + rotate_z[3, 2];
                        rotate[3, 3] = 0;

                    }
                    break;

            }

            Parallel.For(0, objVertices.Count,
                  index =>
                  {
                      double temp_xL = (objVertices[index].x * rotate[0, 0] + objVertices[index].y * rotate[1, 0] + objVertices[index].z * rotate[2, 0]);
                      double temp_yL = (objVertices[index].x * rotate[0, 1] + objVertices[index].y * rotate[1, 1] + objVertices[index].z * rotate[2, 1]);
                      double temp_zL = (objVertices[index].x * rotate[0, 2] + objVertices[index].y * rotate[1, 2] + objVertices[index].z * rotate[2, 2]);

                      objVertices[index].x = temp_xL;
                      objVertices[index].y = temp_yL;
                      objVertices[index].z = temp_zL;

                  });

            //for (int loop = 0; loop < objVertices.Count; loop++)
            //{
            //    double temp_x = (objVertices[loop].x * rotate[0, 0] + objVertices[loop].y * rotate[1, 0] + objVertices[loop].z * rotate[2, 0]);
            //    double temp_y = (objVertices[loop].x * rotate[0, 1] + objVertices[loop].y * rotate[1, 1] + objVertices[loop].z * rotate[2, 1]);
            //    double temp_z = (objVertices[loop].x * rotate[0, 2] + objVertices[loop].y * rotate[1, 2] + objVertices[loop].z * rotate[2, 2]);

            //    objVertices[loop].x = temp_x;
            //    objVertices[loop].y = temp_y;
            //    objVertices[loop].z = temp_z;
            //}

            Parallel.For(0, objVerticesNormals.Count,
                 index =>
                 {
                     double tx = 10.0;
                     double ty = 10.0;
                     double tz = 10.0;

                     double tNx = objVerticesNormals[index].x + tx;
                     double tNy = objVerticesNormals[index].y + ty;
                     double tNz = objVerticesNormals[index].z + tz;

                     double tempxa = tx * rotate[0, 0] + ty * rotate[1, 0] + tz * rotate[2, 0];
                     double tempya = tx * rotate[0, 1] + ty * rotate[1, 1] + tz * rotate[2, 1];
                     double tempza = tx * rotate[0, 2] + ty * rotate[1, 2] + tz * rotate[2, 2];

                     tx = tNx * rotate[0, 0] + tNy * rotate[1, 0] + tNz * rotate[2, 0];
                     ty = tNx * rotate[0, 1] + tNy * rotate[1, 1] + tNz * rotate[2, 1];
                     tz = tNx * rotate[0, 2] + tNy * rotate[1, 2] + tNz * rotate[2, 2];

                     objVerticesNormals[index].x = tx - tempxa;
                     objVerticesNormals[index].y = ty - tempya;
                     objVerticesNormals[index].z = tz - tempza;
                 });

            //for (int loop = 0; loop < objVerticesNormals.Count; loop++)
            //{
            //    double tx = 10.0;
            //    double ty = 10.0;
            //    double tz = 10.0;

            //    double tNx = objVerticesNormals[loop].x + tx;
            //    double tNy = objVerticesNormals[loop].y + ty;
            //    double tNz = objVerticesNormals[loop].z + tz;

            //    double tempxa = tx * rotate[0, 0] + ty * rotate[1, 0] + tz * rotate[2, 0];
            //    double tempya = tx * rotate[0, 1] + ty * rotate[1, 1] + tz * rotate[2, 1];
            //    double tempza = tx * rotate[0, 2] + ty * rotate[1, 2] + tz * rotate[2, 2];

            //    tx = tNx * rotate[0, 0] + tNy * rotate[1, 0] + tNz * rotate[2, 0];
            //    ty = tNx * rotate[0, 1] + tNy * rotate[1, 1] + tNz * rotate[2, 1];
            //    tz = tNx * rotate[0, 2] + tNy * rotate[1, 2] + tNz * rotate[2, 2];

            //    objVerticesNormals[loop].x = tx - tempxa;
            //    objVerticesNormals[loop].y = ty - tempya;
            //    objVerticesNormals[loop].z = tz - tempza;
            //}

            Parallel.For(0, objMeshes[0].objFaces.Count,
                 index =>
                 {

                     //Face face = objMeshes[0].objFaces[index];

                     double tx = 10.0;
                     double ty = 10.0;
                     double tz = 10.0;

                     double tNx = objMeshes[0].objFaces[index].normal.x + tx;
                     double tNy = objMeshes[0].objFaces[index].normal.y + ty;
                     double tNz = objMeshes[0].objFaces[index].normal.z + tz;

                     double tempxa = tx * rotate[0, 0] + ty * rotate[1, 0] + tz * rotate[2, 0];
                     double tempya = tx * rotate[0, 1] + ty * rotate[1, 1] + tz * rotate[2, 1];
                     double tempza = tx * rotate[0, 2] + ty * rotate[1, 2] + tz * rotate[2, 2];

                     double tx2 = tNx * rotate[0, 0] + tNy * rotate[1, 0] + tNz * rotate[2, 0];
                     double ty2 = tNx * rotate[0, 1] + tNy * rotate[1, 1] + tNz * rotate[2, 1];
                     double tz2 = tNx * rotate[0, 2] + tNy * rotate[1, 2] + tNz * rotate[2, 2];

                     objMeshes[0].objFaces[index].normal.x = tx2 - tempxa;
                     objMeshes[0].objFaces[index].normal.y = ty2 - tempya;
                     objMeshes[0].objFaces[index].normal.z = tz2 - tempza;

                 });

            //for (int loopc = 0; loopc < objMeshes[0].objFaces.Count; loopc++)
            //{
            //    Face face = objMeshes[0].objFaces[loopc];

            //    double tx = 10.0;
            //    double ty = 10.0;
            //    double tz = 10.0;

            //    double tNx = face.normal.x + tx;
            //    double tNy = face.normal.y + ty;
            //    double tNz = face.normal.z + tz;

            //    double tempxa = tx * rotate[0, 0] + ty * rotate[1, 0] + tz * rotate[2, 0];
            //    double tempya = tx * rotate[0, 1] + ty * rotate[1, 1] + tz * rotate[2, 1];
            //    double tempza = tx * rotate[0, 2] + ty * rotate[1, 2] + tz * rotate[2, 2];

            //    tx = tNx * rotate[0, 0] + tNy * rotate[1, 0] + tNz * rotate[2, 0];
            //    ty = tNx * rotate[0, 1] + tNy * rotate[1, 1] + tNz * rotate[2, 1];
            //    tz = tNx * rotate[0, 2] + tNy * rotate[1, 2] + tNz * rotate[2, 2];

            //    face.normal.x = tx - tempxa;
            //    face.normal.y = ty - tempya;
            //    face.normal.z = tz - tempza;

            //}

            //for (int loopc = 0; loopc < objMeshes[0].objFaces.Count; loopc++)
            //{
            //    Face face = objMeshes[0].objFaces[loopc];

            //    Vector3 v1 = new Vector3(objVertices[face.vIndex[0]].x, objVertices[face.vIndex[0]].y, objVertices[face.vIndex[0]].z);
            //    Vector3 v2 = new Vector3(objVertices[face.vIndex[1]].x, objVertices[face.vIndex[1]].y, objVertices[face.vIndex[1]].z);
            //    Vector3 v3 = new Vector3(objVertices[face.vIndex[2]].x, objVertices[face.vIndex[2]].y, objVertices[face.vIndex[2]].z);

            //    Vector3 vNormal = new Vector3(0, 0, 0);
            //    vectorMath.calcnormal(v1, v2, v3, ref vNormal);

            //    face.setNormal(vNormal);
            //}

        }

    }
}
