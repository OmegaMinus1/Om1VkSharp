using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShaderGenesis
{
    class vectorMath
    {
        public static double EPSILON = 0.00000000000001;

        //Distance between points
        public static double distance(Vector3 vPoint1, Vector3 vPoint2)
        {
            double distance = Math.Sqrt((vPoint2.x - vPoint1.x) * (vPoint2.x - vPoint1.x) +
                                        (vPoint2.y - vPoint1.y) * (vPoint2.y - vPoint1.y) +
                                        (vPoint2.z - vPoint1.z) * (vPoint2.z - vPoint1.z));
            return distance;
        }

        //Unit Normal
        public static Vector3 normalize(Vector3 vNormal)
        {
            double magnitude = vNormal.magnitude();
            if (magnitude > 0)
            {
                vNormal /= magnitude;
            }
            return vNormal;
        }



        //
        public static Vector3 closestPointOnALine(Vector3 vA, Vector3 vB, Vector3 vPoint)
        {
            Vector3 vVector1 = vPoint - vA;
            Vector3 vVector2 = normalize(vB - vA);
            double d = distance(vA, vB);
            double t = vVector2.dot(vVector1);

            if (t <= 0) return vA;
            if (t >= d) return vB;

            Vector3 vVector3 = vVector2 * t;

            Vector3 vClosestPoint = vA + vVector3;

            return vClosestPoint;
        }


        //ABS
        public static double Absolute(double num)
        {
            if (num < 0.0f)
            {
                return (0.0f - num);
            }

            return num;
        }

        //Distance from a point to a plane
        public static double PlaneDistance(Vector3 Normal, Vector3 Point)
        {
            double distance = -((Normal.x * Point.x) + (Normal.y * Point.y) + (Normal.z * Point.z));

            return distance;
        }

        //
        public static double AngleBetweenVectors(Vector3 Vector1, Vector3 Vector2)
        {
            double dotProduct = dotProd(Vector1, Vector2);
            double vectorsMagnitude = Vector1.magnitude() * Vector2.magnitude();
            double angle = Math.Acos(dotProduct / vectorsMagnitude);

            if (double.IsNaN(angle))
            {
                return 0;
            }

            return (angle);
        }

        //Max value
        public static Vector3 max(Vector3 Vector1, Vector3 Vector2)
        {
            if (Vector1 >= Vector2)
            {
                return Vector1;
            }
            return Vector2;
        }

        public static Vector3 min(Vector3 Vector1, Vector3 Vector2)
        {
            if (Vector1 <= Vector2)
            {
                return Vector1;
            }
            return Vector2;
        }

        public static bool IsBackFace(Vector3 normal, Vector3 lineOfSight)
        {
            return normal.dot(lineOfSight) < 0;
        }

        public static bool IsPerpendicular(Vector3 v1, Vector3 v2)
        {
            return v1.dot(v2) == 0;
        }

        //Dot product
        public static double dotProd(Vector3 vVector1, Vector3 vVector2)
        {
            return ((vVector1.x * vVector2.x) + (vVector1.y * vVector2.y) + (vVector1.z * vVector2.z));
        }

        //Cross product
        public static Vector3 cross(Vector3 vVector1, Vector3 vVector2)
        {
            Vector3 vNormal = new Vector3(((vVector1.y * vVector2.z) - (vVector1.z * vVector2.y)),
                                          ((vVector1.z * vVector2.x) - (vVector1.x * vVector2.z)),
                                          ((vVector1.x * vVector2.y) - (vVector1.y * vVector2.x)));

            return vNormal;
        }

        public static void calcnormal(Vector3 a, Vector3 b, Vector3 c, ref Vector3 outVector)
        {
            //Vector3 dir = cross(b - a, c - a);
            Vector3 dir = cross(c - a, b - a);
            //Vector3 dir = cross(a - b, b - c);
            dir.normalize();

            outVector = dir;
        }


        public static double MixedProduct(Vector3 v1, Vector3 v2, Vector3 v3)
        {
            return dotProd(cross(v1, v2), v3);
        }

        public static double SumComponents(Vector3 v1)
        {
            return (v1.x + v1.y + v1.z);
        }

        public static Vector3 pow(Vector3 v1, double power)
        {
            return (new Vector3(Math.Pow(v1.x, power), Math.Pow(v1.y, power), Math.Pow(v1.z, power)));
        }

        public static Vector3 SqrtComponents(Vector3 v1)
        {
            return (new Vector3(Math.Sqrt(v1.x), Math.Sqrt(v1.y), Math.Sqrt(v1.z)));
        }

        public static Vector3 SqrComponents(Vector3 v1)
        {
            return (new Vector3(v1.x * v1.x, v1.y * v1.y, v1.z * v1.z));
        }

        public static double SumComponentSqrs(Vector3 v1)
        {
            Vector3 v2 = SqrComponents(v1);
            return SumComponents(v2);
        }

        public static double dot3(double x1, double y1, double z1, double x2, double y2, double z2)
        {
            return ((x1 * x2) + (y1 * y2) + (z1 * z2));
        }

        public static void RotateX(ref Vector3 v, double a)
        {
            double xX = v.x;
            double xY = (v.y * Math.Cos(a)) + (v.z * -Math.Sin(a));
            double xZ = (v.y * Math.Sin(a)) + (v.z * Math.Cos(a));

            v.x = xX;
            v.y = xY;
            v.z = xZ;
        }

        public static void RotateY(ref Vector3 v, double a)
        {

            double xX = (v.x * Math.Cos(a)) + (v.z * Math.Sin(a));
            double xY = v.y;
            double xZ = (v.x * -Math.Sin(a)) + (v.z * Math.Cos(a));

            v.x = xX;
            v.y = xY;
            v.z = xZ;
        }

        public static void RotateZ(ref Vector3 v, double a)
        {

            double xX = (v.x * Math.Cos(a)) + (v.y * -Math.Sin(a));
            double xY = (v.x * Math.Sin(a)) + (v.y * Math.Cos(a));
            double xZ = v.z;

            v.x = xX;
            v.y = xY;
            v.z = xZ;
        }

        public static void Reflect(double inx, double iny, double inz, double mirrorx, double mirrory, double mirrorz, ref double outx, ref double outy, ref double outz)
        {
            double c1 = -vectorMath.dot3(mirrorx, mirrory, mirrorz, inx, iny, inz);

            //Rl = V + (2 * N * c1 )
            outx = -(inx + (2.0f * mirrorx * c1));
            outy = -(iny + (2.0f * mirrory * c1));
            outz = -(inz + (2.0f * mirrorz * c1));
        }

        public static void Reflect(Vector3 input, Vector3 mirror, ref Vector3 output)
        {
            double c1 = -mirror.dot(input);

            //Rl = V + (2 * N * c1 )

            output.x = -(input.x + (2.0f * mirror.x * c1));
            output.y = -(input.y + (2.0f * mirror.y * c1));
            output.z = -(input.z + (2.0f * mirror.z * c1));
        }

        public static Vector3 Reflect(Vector3 input, Vector3 mirror)
        {
            double c1 = -mirror.dot(input);
            Vector3 output = new Vector3(0.0f, 0.0f, 0.0f);
            //Rl = V + (2 * N * c1 )

            output.x = -(input.x + (2.0f * mirror.x * c1));
            output.y = -(input.y + (2.0f * mirror.y * c1));
            output.z = -(input.z + (2.0f * mirror.z * c1));

            return output;
        }

        public static void Refract(double n1, double n2, double inx, double iny, double inz, double mirrorx, double mirrory, double mirrorz, ref double outx, ref double outy, ref double outz)
        {
            double c1 = -vectorMath.dot3(mirrorx, mirrory, mirrorz, inx, iny, inz);
            double n = n1 / n2;

            double c2 = Math.Sqrt(1.0 - n * n * (1.0 - c1 * c1));
            outx = (n * inx) + (n * c1 - c2) * mirrorx;
            outy = (n * iny) + (n * c1 - c2) * mirrory;
            outz = (n * inz) + (n * c1 - c2) * mirrorz;
        }

        public static void Refract(double n1, double n2, Vector3 input, Vector3 mirror, ref Vector3 output)
        {
            double c1 = -mirror.dot(input);
            double n = n1 / n2;

            double c2 = Math.Sqrt(1.0f - n * n * (1.0f - c1 * c1));

            output.x = (n * input.x) + (n * c1 - c2) * mirror.x;
            output.y = (n * input.y) + (n * c1 - c2) * mirror.y;
            output.z = (n * input.z) + (n * c1 - c2) * mirror.z;
        }

        public static double GetCosAngleV1V2(double v1x, double v1y, double v1z, double v2x, double v2y, double v2z)
        {
            // cos(t) = (v.w) / (|v|.|w|) = (v.w) / 1
            return vectorMath.dot3(v1x, v1y, v1z, v2x, v2y, v2z);
        }

        public static bool InsidePolygon(Vector3 vIntersection, Vector3[] Poly, long verticeCount)
        {
            const double MATCH_FACTOR = 0.99f;
            double Angle = 0.0f;
            Vector3 vA, vB;

            for (int i = 0; i < verticeCount; i++)
            {
                vA = Poly[i] - vIntersection;

                vB = Poly[(i + 1) % verticeCount] - vIntersection;

                Angle += AngleBetweenVectors(vA, vB);
            };

            if (Angle >= (MATCH_FACTOR * (2.0f * Math.PI)))
            //if (Angle >= 2.0f * Math.PI)
            //if (Angle >= Math.PI)
            
                return true;

            return false;
        }

        //public static bool PointInTriangleVectors(ref Vector3 A, Vector3 B, Vector3 C, ref Vector3 P)
        //{

        //    if (PointOnSameSide2(ref P, ref A, ref B, ref C) && PointOnSameSide2(ref P, ref B, ref A, ref C) && PointOnSameSide2(ref P, ref C, ref A, ref B))
        //    {
        //        if (Math.Abs(((A - P) * ((A - B) / (A - C)))) <= 0.01f)
        //        {
        //            return true;
        //        }
        //    }

        //    return false;
        //}

        public static bool PointInTriangleVectors2(ref Vector3 A, Vector3 B, Vector3 C, ref  Vector3 P)
        {

            if (PointOnSameSide2(ref P, ref A, ref B, ref C) && PointOnSameSide2(ref P, ref B, ref A, ref C) && PointOnSameSide2(ref P, ref C, ref A, ref B))
            {
                double dAPx = A.x - P.x;
                double dAPy = A.y - P.y;
                double dAPz = A.z - P.z;

                double dABx = A.x - B.x;
                double dABy = A.y - B.y;
                double dABz = A.z - B.z;

                double dACx = A.x - C.x;
                double dACy = A.y - C.y;
                double dACz = A.z - C.z;

                double cross1x = dABy * dACz - dABz * dACy;
                double cross1y = dABz * dACx - dABx * dACz;
                double cross1z = dABx * dACy - dABy * dACx;

                double num = (dAPx * cross1x) + (dAPy * cross1y) + (dAPz * cross1z);
                if (num < 0.0f)
                {
                    if ((0.0f - num) <= 0.1f)
                    {
                        return true;
                    }
                }
                else
                {
                    if (num <= 0.1f)
                    {
                        return true;
                    }
                }

                //if (vectorMath.Absolute(dot3(dAPx, dAPy, dAPz, cross1x, cross1y, cross1z)) <= 0.1f)
                //{
                //    return true;
                //}

            }

            return false;
        }

        public static bool PointInTriangleVectors3(ref Vector3 A, Vector3 B, Vector3 C, ref  double Px, ref  double Py, ref  double Pz)
        {

            if (PointOnSameSide3(ref Px, ref Py, ref Pz, ref A, ref B, ref C) && PointOnSameSide3(ref Px, ref Py, ref Pz, ref B, ref A, ref C) && PointOnSameSide3(ref Px, ref Py, ref Pz, ref C, ref A, ref B))
            {
                double dAPx = A.x - Px;
                double dAPy = A.y - Py;
                double dAPz = A.z - Pz;

                double dABx = A.x - B.x;
                double dABy = A.y - B.y;
                double dABz = A.z - B.z;

                double dACx = A.x - C.x;
                double dACy = A.y - C.y;
                double dACz = A.z - C.z;

                double cross1x = dABy * dACz - dABz * dACy;
                double cross1y = dABz * dACx - dABx * dACz;
                double cross1z = dABx * dACy - dABy * dACx;

                double num = (dAPx * cross1x) + (dAPy * cross1y) + (dAPz * cross1z);
                if (num < 0.0f)
                {
                    if ((0.0f - num) <= 0.1f)
                    {
                        return true;
                    }
                }
                else
                {
                    if (num <= 0.1f)
                    {
                        return true;
                    }
                }

                //if (vectorMath.Absolute(dot3(dAPx, dAPy, dAPz, cross1x, cross1y, cross1z)) <= 0.1f)
                //{
                //    return true;
                //}

            }

            return false;
        }


        public static bool PointOnSameSide2(ref Vector3 p1, ref Vector3 p2, ref Vector3 A, ref Vector3 B)
        {
            double dBAx = B.x - A.x;
            double dBAy = B.y - A.y;
            double dBAz = B.z - A.z;

            double dP1Ax = p1.x - A.x;
            double dP1Ay = p1.y - A.y;
            double dP1Az = p1.z - A.z;

            double dP2Ax = p2.x - A.x;
            double dP2Ay = p2.y - A.y;
            double dP2Az = p2.z - A.z;

            double cross1x = dBAy * dP1Az - dBAz * dP1Ay;
            double cross1y = dBAz * dP1Ax - dBAx * dP1Az;
            double cross1z = dBAx * dP1Ay - dBAy * dP1Ax;

            double cross2x = dBAy * dP2Az - dBAz * dP2Ay;
            double cross2y = dBAz * dP2Ax - dBAx * dP2Az;
            double cross2z = dBAx * dP2Ay - dBAy * dP2Ax;

            if ((cross1x * cross2x) + (cross1y * cross2y) + (cross1z * cross2z) >= 0)
            //if (dot3(cross1x, cross1y, cross1z, cross2x, cross2y, cross2z) >= 0)
            {
                return true;
            }

            return false;

        }


        public static bool PointOnSameSide3(ref double p1x, ref double p1y, ref double p1z, ref Vector3 p2, ref Vector3 A, ref Vector3 B)
        {
            double dBAx = B.x - A.x;
            double dBAy = B.y - A.y;
            double dBAz = B.z - A.z;

            double dP1Ax = p1x - A.x;
            double dP1Ay = p1y - A.y;
            double dP1Az = p1z - A.z;

            double dP2Ax = p2.x - A.x;
            double dP2Ay = p2.y - A.y;
            double dP2Az = p2.z - A.z;

            double cross1x = dBAy * dP1Az - dBAz * dP1Ay;
            double cross1y = dBAz * dP1Ax - dBAx * dP1Az;
            double cross1z = dBAx * dP1Ay - dBAy * dP1Ax;

            double cross2x = dBAy * dP2Az - dBAz * dP2Ay;
            double cross2y = dBAz * dP2Ax - dBAx * dP2Az;
            double cross2z = dBAx * dP2Ay - dBAy * dP2Ax;

            if ((cross1x * cross2x) + (cross1y * cross2y) + (cross1z * cross2z) >= 0)
            //if (dot3(cross1x, cross1y, cross1z, cross2x, cross2y, cross2z) >= 0)
            {
                return true;
            }

            return false;

        }

        //public static bool PointOnSameSide(Vector3 p1, Vector3 p2, Vector3 A, Vector3 B)
        //{

        //    if (((B - A) / (p1 - A)) * ((B - A) / (p2 - A)) >= 0)
        //    {

        //        return true;
        //    }

        //    return false;

        //}


    }
}
