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

namespace LostLives
{
    class Circle
    {
        public Vector2 center;
        public float radius;

        public Circle(Vector2 centerPoint, float _radius)
        {
            center = centerPoint;
            radius = _radius;
        }

        public Vector2[] Intersects(Line line)
        {
            Vector2[] solutions;
            float[] solutionXs;
            float A = line.multipliers.X;
            float B = line.multipliers.Y;
            float C = line.constant;
            float Discriminant = (float)(Math.Pow((2 * A * C) / B, 2) - (4 + 4 * Math.Pow(A, 2)) * (Math.Pow(C / B, 2) - Math.Pow(center.X, 2) - Math.Pow(center.Y, 2) - Math.Pow(radius, 2)));

            if(Discriminant < 0)
            {
                return null;
            }
            else if(Discriminant > 0)
            {
                solutionXs = new float[2];
                solutions = new Vector2[2];
            }
            else
            {
                solutionXs = new float[1];
                solutions = new Vector2[1];
            }

            for(int i = 0; i < solutionXs.Length; i++)
            {
                solutionXs[i] = ((-2 * A * C) / B + (float)Math.Sqrt(Discriminant) * (1 - 2 * i)) / (2 + 2 * (float)Math.Pow(A, 2));
            }

            for (int i = 0; i < solutions.Length; i++)
            {
                solutions[i] = new Vector2(solutionXs[i], A * solutionXs[i] - (C / B));
            }

            return solutions;
        }
    }
}
