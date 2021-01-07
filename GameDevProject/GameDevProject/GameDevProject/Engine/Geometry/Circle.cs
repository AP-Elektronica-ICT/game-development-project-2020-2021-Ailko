#region Includes
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
#endregion

namespace GameDevProject.Engine.Geometry
{
    public class Circle
    {
        #region variables
        public Vector2 center;
        public float radius;
        #endregion

        #region constructors
        public Circle(Vector2 centerPoint, float _radius)
        {
            center = centerPoint;
            radius = _radius;
        }
        public Circle(Point centerPoint, float _radius)
        {
            center = new Vector2(centerPoint.X, centerPoint.Y);
            radius = _radius;
        }
        #endregion
        #region Intersect
        public Vector2[] Intersects(Line line)
        {
            Vector2[] solutions;
            float[] solutionXs;
            float A = line.slope;
            float B = line.slopeInterceptConstant;
            float h = center.X;
            float k = center.Y;
            float r = radius;

            #region  calculating discriminant
            float a = 1 + (float)Math.Pow(A, 2);
            float b = 2 * (-h + A * B - A * k);
            float c = (float)Math.Pow(h, 2) + (float)Math.Pow(k, 2) - (float)Math.Pow(r, 2) + (float)Math.Pow(B, 2) - 2 * B * k;
            float Discriminant = (float)Math.Pow(b, 2) - 4 * a * c;

            #endregion
            if (Discriminant < 0)
            {
                return null;
            }
            else if (Discriminant > 0)
            {
                solutionXs = new float[2];
                solutions = new Vector2[2];
            }
            else
            {
                solutionXs = new float[1];
                solutions = new Vector2[1];
            }

            #region find solution X(s)
            for (int i = 0; i < solutionXs.Length; i++)
            {
                solutionXs[i] = (-b + (1 - 2 * i) * (float)Math.Sqrt(Discriminant)) / (2 * a);
            }
            #endregion
            for (int i = 0; i < solutions.Length; i++)
            {
                solutions[i] = new Vector2(solutionXs[i], A * solutionXs[i] + B);
            }

            return solutions;
        }
        #endregion
        #region object overrides
        public override string ToString()
        {
            return $"(x - {center.X})^2 + (y - {center.Y})^2 = {radius}^2";
        }
        #endregion
    }
}
