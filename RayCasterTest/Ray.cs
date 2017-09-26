using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rayCasterTest
{
    public class Ray
    {
        public Map m_map;
        public double sin;
        public double cos;
        public List<StepInfo> steps;

        Vector3 sunPos = new Vector3(0, 0, 0);

        public Ray(Map map, StepInfo origin, double Sin, double Cos, double range)
        {
            m_map = map;
            sin = Sin;
            cos = Cos;
            steps = new List<StepInfo>();

            Cast(origin, range);

        }

        public void Cast(StepInfo origin, double range)
        {
            StepInfo stepX = Step(sin, cos, origin.x, origin.y, false);
            StepInfo stepY = Step(cos, sin, origin.y, origin.x, true);

            StepInfo nextStep = stepX.length2 < stepY.length2 ? Inspect(stepX, 1, 0, origin.distance, stepX.y) : Inspect(stepY, 0, 1, origin.distance, stepY.x);

            steps.Add(origin);

            if (nextStep.distance < range)
            {
                //////////////////

                //Cast ray from step point to next cell point and collide with primative
                bool PrimativeHit = false;
                double PrimativeDistance = 0.0;

                Vector3 PrimativeHitColor = new Vector3(1.0f, 1.0f, 1.0f);

                int cell = m_map.GetCellId(origin.CellX, origin.CellY);
                if (cell > -1 && m_map.m_Grid[cell].primative != null)
                {
                    ProcessPrimative(origin, nextStep, ref PrimativeHit, ref PrimativeDistance, PrimativeHitColor, cell);

                    if (PrimativeHit == true)
                    {
                        //Calc Distance from step.x step.y to HitPoint.X HitPoint.Y
                        origin.distance += PrimativeDistance;
                        origin.primativeHit = PrimativeHit;
                        origin.primativeHitColor.x = PrimativeHitColor.x;
                        origin.primativeHitColor.y = PrimativeHitColor.y;
                        origin.primativeHitColor.z = PrimativeHitColor.z;
                    }
                    else
                    {
                        origin.primativeHit = PrimativeHit;
                    }

                }
                //////////////////

                Cast(nextStep, range);
            }
        }

        public StepInfo Step(double rise, double run, double x, double y, bool inverted)
        {
            if (run == 0) return new StepInfo(0, 0, 0, 0, double.MaxValue, 0, 0, false);

            double dx = run > 0 ? Math.Floor(x + 1) - x : Math.Ceiling(x - 1) - x;
            double dy = dx * (rise / run);

            return new StepInfo(inverted ? y + dy : x + dx, inverted ? x + dx : y + dy, 0, 0, dx * dx + dy * dy, 0, 0, false);
        }

        public StepInfo Inspect(StepInfo step, double shiftX, double shiftY, double distance, double offset)
        {
            double dx = cos < 0 ? shiftX : 0;
            double dy = sin < 0 ? shiftY : 0;

            step.CellX = step.x - dx;
            step.CellY = step.y - dy;

            step.height = m_map.Get(step.x - dx, step.y - dy);
            step.distance = distance + Math.Sqrt(step.length2);

            int cell = m_map.GetCellId(step.CellX, step.CellY);
            if (cell >-1 && m_map.m_Grid[cell].primative != null)
            {
                step.hasPrimative = true;
            }

            if (shiftX == 1)
                step.shading = cos < 0 ? 2 : 0;
            else
                step.shading = sin < 0 ? 2 : 1;

            step.offset = offset - Math.Floor(offset);

            return step;
        }
        
        private void ProcessPrimative(StepInfo step, StepInfo stepNext, ref bool PrimativeHit, ref double PrimativeDistance, Vector3 PrimativeHitColor, int cell)
        {
            //if (m_map.m_Grid[cell].primative != null)
            {
                double objDistance = 100000.0f;
                int objIndex = -1;

                Model prim = m_map.m_Grid[cell].primative;

                int cellX1 = (int)(step.CellX * m_map.m_Size);
                int cellX2 = (int)(cellX1 + m_map.m_Size);

                int cellY1 = (int)(step.CellY * m_map.m_Size);
                int cellY2 = (int)(cellY1 + m_map.m_Size);

                int centerCellX = (int)(cellX1 + (m_map.m_Size / 2));
                int centerCellY = (int)(cellY1 + (m_map.m_Size / 2));

                bool hitTest = false;
                double hitDistance = 0.0000;
                Vector3 hitPoint = new Vector3(0, 0, 0);
                Vector3 hitNormal = new Vector3(0, 0, 0);
                Vector3 hitColor = new Vector3(0, 0, 0);
                int hitFaceIndex = -1;
                int hitMeshIndex = -1;
                int MeshIndex = -1;
                int FaceIndex = -1;

                Vector3 objColor = new Vector3(0.0f, 0.0f, 0.0f);
                Vector3 objSpecular = new Vector3(0.0f, 0.0f, 0.0f);
                Vector3 objOcclusion = new Vector3(1.0f, 1.0f, 1.0f);
                Vector3 objEmissive = new Vector3(0.0f, 0.0f, 0.0f);
                Vector3 objPoint = new Vector3(0.0f, 0.0f, 0.0f);
                Vector3 objNormal = new Vector3(0.0f, 0.0f, 0.0f);



                double objU = 0.0f;
                double objV = 0.0f;

                Vector3 RayOrign = new Vector3(step.x * (int)m_map.m_Size, step.y * (int)m_map.m_Size, 0);
                Vector3 RayEnd = new Vector3(stepNext.x * (int)m_map.m_Size, stepNext.y * (int)m_map.m_Size, 0);

                double hitFaceU = 0.0f;
                double hitFaceV = 0.0f;

                prim.traceRay(RayOrign, RayEnd, ref hitTest, ref hitDistance, ref hitPoint, ref hitNormal, ref hitColor, ref hitFaceIndex, ref hitMeshIndex, ref hitFaceU, ref hitFaceV);

                if (hitTest && hitDistance < objDistance)
                {
                    double colorFactor = 0.0f;

                    PrimativeHit = true;
                    PrimativeDistance = hitDistance / 32.0;

                    objDistance = hitDistance;
                    objIndex = 0;// modelLoop;

                    objColor.x = 1.0f;//hitColor.x;
                    objColor.y = 0.0f;//hitColor.y;
                    objColor.z = 0.0f;//hitColor.z;

                    objPoint.x = hitPoint.x;
                    objPoint.y = hitPoint.y;
                    objPoint.z = hitPoint.z;

                    objNormal.x = hitNormal.x;
                    objNormal.y = hitNormal.y;
                    objNormal.z = hitNormal.z;

                    FaceIndex = hitFaceIndex;
                    MeshIndex = hitMeshIndex;

                    Int32 hitFaceUInt = (Int32)hitFaceU;
                    Int32 hitFaceVInt = (Int32)hitFaceV;
                    objU = (hitFaceU - hitFaceUInt);
                    objV = 1.0f - (hitFaceV - hitFaceVInt);

                    //Calc Texture Sample, Shading and Lighting here
                    if (objIndex > -1)
                    {
                        //Map uv to 0.0 - 1.0 from -1.0 - 1.0
                        //sunPos
                        Vector3 lightVector = new Vector3(objPoint.x, objPoint.y, objPoint.z);

                        Vector3 LightToIntersectionVector = new Vector3(sunPos.x - objPoint.x, sunPos.y - objPoint.y, sunPos.z - objPoint.z);
                        LightToIntersectionVector.normalize();

                        //Vector3 lightVectorNormal = new Vector3(LightToIntersectionVector.x, LightToIntersectionVector.y, LightToIntersectionVector.z); ;

                        Vector3 lightColor = new Vector3(1.0f, 1.0f, 1.0f);

                        double Ia = 0.10f; //Power

                        double AmbientRed = Ia;// *objColor.x; //Object True Color @ 10%
                        double AmbientGreen = Ia;// *objColor.y; //Object True Color @ 10%
                        double AmbientBlue = Ia;// *objColor.z; //Object True Color @ 10%

                        //Calc Diffuse
                        double Id = 1.0f;
                        LightToIntersectionVector.invert();
                        colorFactor = (objNormal.dot(LightToIntersectionVector));

                        //double DiffuseRed = Id * Math.Max(colorFactor, 0.0);// *objColor.x;
                        //double DiffuseGreen = Id * Math.Max(colorFactor, 0.0);// *objColor.y;
                        //double DiffuseBlue = Id * Math.Max(colorFactor, 0.0);// *objColor.z;

                        PrimativeHitColor.x = Math.Min(AmbientRed + (Id * Math.Max(colorFactor, 0.0)), 1.0);// *objColor.x;
                        PrimativeHitColor.y = Math.Min(AmbientGreen + (Id * Math.Max(colorFactor, 0.0)), 1.0);// *objColor.x;
                        PrimativeHitColor.z = Math.Min(AmbientBlue + (Id * Math.Max(colorFactor, 0.0)), 1.0);// *objColor.x;
                    }
                }

            }
            //else
            //{
            //    PrimativeHit = true;
            //}
        }

    }

}
