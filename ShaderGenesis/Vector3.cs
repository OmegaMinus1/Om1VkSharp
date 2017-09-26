using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShaderGenesis
{
    public class Vector3f
    {
        public float x = 0, y = 0, z = 0, align = 0;

        public Vector3f(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }
        public class Vector3
    {
        public double x = 0, y = 0, z = 0, align = 0;

        public Vector3(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public void Set(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }


        //Invert
        public void invert()
        {
            this.x = -this.x;
            this.y = -this.y;
            this.z = -this.z;
        }

        //Dot product
        public static double operator *(Vector3 A, Vector3 B)
        {
            double dot = A.x * B.x + A.y * B.y + A.z * B.z;

            return (dot);
        }

        public double dot(Vector3 A)
        {
            double dot = (x * A.x) + (y * A.y) + (z * A.z);
            return (dot);
        }

        //Addition
        public static Vector3 operator +(Vector3 A, Vector3 B)
        {
            Vector3 sum = new Vector3(A.x + B.x, A.y + B.y, A.z + B.z);
            return (sum);
        }

        //Subtraction
        public static Vector3 operator -(Vector3 A, Vector3 B)
        {
            Vector3 sum = new Vector3(A.x - B.x, A.y - B.y, A.z - B.z);
            return (sum);
        }


        //Subtraction
        public static Vector3 operator -(Vector3 A, double B)
        {
            Vector3 sum = new Vector3(A.x - B, A.y - B, A.z - B);
            return (sum);
        }

        //Scaler scale
        public static Vector3 operator *(Vector3 A, double value)
        {
            Vector3 sum = new Vector3(A.x * value, A.y * value, A.z * value);
            return (sum);
        }

        //Scaler divide
        public static Vector3 operator /(Vector3 A, double value)
        {
            Vector3 sum = new Vector3(A.x / value, A.y / value, A.z / value);
            return (sum);
        }

        //Cross Product
        public static Vector3 operator /(Vector3 A, Vector3 B)
        {
            Vector3 CrossProd = new Vector3(A.y * B.z - A.z * B.y, A.z * B.x - A.x * B.z, A.x * B.y - A.y * B.x);
            return (CrossProd);
        }

        //Magnitude
        public double magnitude()
        {
            return (double)Math.Sqrt(x * x + y * y + z * z);
        }

        //Unit Normal
        public void normalize()
        {
            double length = magnitude();
            if (length > 0)
            {
                this.x /= length;
                this.y /= length;
                this.z /= length;
            }
        }

        public static bool operator <(Vector3 v1, Vector3 v2)
        {
            return v1.magnitude() < v2.magnitude();
        }
        public static bool operator <=(Vector3 v1, Vector3 v2)
        {
            return v1.magnitude() <= v2.magnitude();
        }
        public static bool operator >(Vector3 v1, Vector3 v2)
        {
            return v1.magnitude() > v2.magnitude();
        }
        public static bool operator >=(Vector3 v1, Vector3 v2)
        {
            return v1.magnitude() >= v2.magnitude();
        }



        void RotateX(double a)
        {
            double xX = x;
            double xY = (y * Math.Cos(a)) + (z * -Math.Sin(a));
            double xZ = (y * Math.Sin(a)) + (z * Math.Cos(a));

            x = xX;
            y = xY;
            z = xZ;
        }

        void RotateY(double a)
        {

            double xX = (x * Math.Cos(a)) + (z * Math.Sin(a));
            double xY = y;
            double xZ = (x * -Math.Sin(a)) + (z * Math.Cos(a));

            x = xX;
            y = xY;
            z = xZ;
        }

        void RotateZ(double a)
        {

            double xX = (x * Math.Cos(a)) + (y * -Math.Sin(a));
            double xY = (x * Math.Sin(a)) + (y * Math.Cos(a));
            double xZ = z;

            x = xX;
            y = xY;
            z = xZ;
        }


        internal string ToString(string p)
        {
            return (x.ToString() + "|" + y.ToString() + "|" + z.ToString());
        }
    }

}
