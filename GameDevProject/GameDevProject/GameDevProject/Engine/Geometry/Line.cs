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

#region Origin code
//Code copied from my spare time project 'Geometry2D'. Sourcecode can be provided if proof is needed.
#endregion

namespace GameDevProject.Engine.Geometry
{
    public class Line
    {
        #region variables
        protected internal Vector2 P1;
        protected internal Vector2 P2;
        protected internal float twoPointFormY;
        protected internal float twoPointFormX;
        protected internal float twoPointFormC;

        public float slope;
        public float slopeInterceptConstant;
        public Vector2 yIntercept;
        public Vector2 xIntercept;
        #endregion

        #region constructors
        public Line(Vector2 p, float slope)
        {
            P1 = p;
            if (slope != 0)
            {
                P2 = new Vector2(p.X + 1, (p.X + 1) * slope + p.Y - p.X * slope);
            }
            else
            {
                P2 = new Vector2(p.X + 1, p.Y);
            }
            constructAttributes();
        }
        public Line(Vector2 p1, Vector2 p2)
        {
            if (p1 != p2)
            {
                P1 = p1;
                P2 = p2;
                constructAttributes();
            }
            else
            {
                try
                {
                    throw new Exception("Vector2s cannot be the same point.");
                }
                catch
                {

                }
            }
        }
        #region support methods
        private void constructAttributes()
        {
            twoPointFormY = P2.X - P1.X;
            twoPointFormX = P1.Y - P2.Y;
            twoPointFormC = P1.X * P2.Y - P2.X * P1.Y;
            if (twoPointFormY != 0)
            {
                slope = -twoPointFormX / twoPointFormY;
                slopeInterceptConstant = -twoPointFormC / twoPointFormY;
            }
            else
            {
                slope = float.PositiveInfinity;
            }
            if (float.IsFinite(slope))
                yIntercept = new Vector2(0, slopeInterceptConstant);
            if (slope != 0)
                xIntercept = new Vector2(-slopeInterceptConstant / slope, 0);
        }
        #endregion
        #endregion
        #region get point at ...
        public virtual Vector2 whereX(float x)
        {
            if (float.IsInfinity(slope))
                return new Vector2(x, slope * x - slopeInterceptConstant);
            return Vector2.Zero;
        }
        public virtual Vector2 whereY(float y)
        {
            if (slope != 0)
                return new Vector2((y - slopeInterceptConstant) / slope, y);
            return Vector2.Zero;
        }
        #endregion
        #region contains
        public virtual bool Contains(Vector2 p)
        {
            if (p.Y == slope * p.X + slopeInterceptConstant)
                return true;
            return false;
        }
        #endregion
        #region intersects
        public Vector2 Intersects(Line line)
        {
            if (slope != line.slope)
            {
                float x = 0;
                if (!float.IsInfinity(slope))
                    x = (line.slopeInterceptConstant - slopeInterceptConstant) / (slope - line.slope);
                else
                    x = P1.X;
                return new Vector2(x, slope * x + slopeInterceptConstant);
            }
            return Vector2.Zero;
        }
        public Vector2[] Intersects(Rectangle rect)
        {
            List<Vector2> solutions = new List<Vector2>();
            Vector2[] rectPoints = new Vector2[] { new Vector2(rect.Left, rect.Top), new Vector2(rect.Right, rect.Top), new Vector2(rect.Left, rect.Bottom), new Vector2(rect.Right, rect.Bottom) };
            solutions.Add(Intersects(new Line(rectPoints[0], rectPoints[1])));
            solutions.Add(Intersects(new Line(rectPoints[0], rectPoints[2])));
            solutions.Add(Intersects(new Line(rectPoints[3], rectPoints[1])));
            solutions.Add(Intersects(new Line(rectPoints[3], rectPoints[2])));
            solutions.Remove(Vector2.Zero);
            return solutions.ToArray();
        }
        #endregion
        #region Object overrides
        public override string ToString()
        {
            return $"y = {slope}x + {slopeInterceptConstant}";
        }
        public override bool Equals(object obj)
        {
            if ((object)obj != null)
            {
                if (slope == ((Line)obj).slope && slopeInterceptConstant == ((Line)obj).slopeInterceptConstant)
                    return true;
            }
            return false;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        #endregion
        #region operators
        public static Line operator +(Line line, Vector2 v)
        {
            return new Line(line.P1 + v, line.P2 + v);
        }
        public static Line operator -(Line line, Vector2 v)
        {
            return new Line(line.P1 - v, line.P2 - v);
        }
        public static bool operator ==(Line line1, Line line2)
        {
            if ((object)line1 != null)
                return line1.Equals(line2);
            if ((object)line2 != null)
                return false;
            return true;
        }
        public static bool operator !=(Line line1, Line line2)
        {
            return !(line1 == line2);
        }
        #endregion
    }
}