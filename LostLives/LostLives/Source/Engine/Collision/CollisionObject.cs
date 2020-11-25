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
    enum collisionAngle { Horizontale, Vertical, Diagonal, None }
    public interface CollisionObject
    {
        #region collision methods
        public static Vector2 CollisionSpecifics(CollisionObject obj1, CollisionObject obj2)
        {
            #region creation variables
            Vector2 collisionVector = Vector2.Zero;

            collisionAngle angle = collisionAngle.None;

            Rectangle obj1Box = obj1.GetCollisionBox();
            Rectangle obj2Box = obj2.GetCollisionBox();
            Vector2 obj1Midpoint = new Vector2(obj1Box.X + obj1Box.Width / 2, obj1Box.Y + obj1Box.Height / 2);
            Vector2 speed = obj1.GetSpeed();

            float smallestWidth;
            float smallestHeight;
            if (obj1Box.Width < obj2Box.Width)
                smallestWidth = obj1Box.Width;
            else
                smallestWidth = obj2Box.Width;

            if (obj1Box.Height < obj2Box.Height)
                smallestHeight = obj1Box.Height;
            else
                smallestHeight = obj2Box.Height;
            #endregion
            #region determine collision angle
            if (obj1Midpoint.Y < obj2Box.Top || obj1Midpoint.Y > obj2Box.Bottom)
            {
                if (obj1Midpoint.X < obj2Box.Left || obj1Midpoint.X > obj2Box.Right)
                {
                    angle = collisionAngle.Diagonal;
                }
                else
                {
                    angle = collisionAngle.Vertical;
                }
            }
            else
            {
                angle = collisionAngle.Horizontale;
            }
            #endregion
            #region collision
            #region vertical collision
            if (angle == collisionAngle.Vertical)
            {
                if(obj1Midpoint.Y < obj2Box.Top && speed.Y > 0)
                    collisionVector.Y -= obj1.GetSpeed().Y * (1 + (obj2.GetBounciness() / 10) + Math.Abs(obj1Box.Bottom - obj2Box.Top) / smallestHeight);
                if(obj1Midpoint.Y > obj2Box.Bottom && speed.Y < 0)
                    collisionVector.Y -= obj1.GetSpeed().Y * (1 + (obj2.GetBounciness() / 10) + Math.Abs(obj2Box.Bottom - obj1Box.Top) / smallestHeight);
            }
            #endregion
            #region horizontal collision
            if (angle == collisionAngle.Horizontale)
            {
                if (obj1Midpoint.X < obj2Box.Left && speed.X > 0 && (obj1Midpoint.Y + (obj1Box.Height * 0.9f) / 2 > obj2Box.Top || obj1Midpoint.Y - (obj1Box.Height * 0.9f) / 2 < obj2Box.Bottom))
                    collisionVector.X -= speed.X * (1 + (obj2.GetBounciness() / 10) + Math.Abs(obj2Box.Left - obj1Box.Right) / smallestWidth);
                if (obj1Midpoint.X > obj2Box.Right && speed.X < 0)
                    collisionVector.X -= speed.X * (1 + (obj2.GetBounciness() / 10) + Math.Abs(obj1Box.Left - obj2Box.Right) / smallestWidth);
            }
            #endregion
            #region diagonal collision
            if(angle == collisionAngle.Diagonal)
            {
                Vector2 centerPoint = Vector2.Zero;
                Vector2 closestPointObj1 = Vector2.Zero;
                if (obj1Midpoint.Y < obj2Box.Top)
                {
                    centerPoint.Y = obj2Box.Top;
                    closestPointObj1.Y = obj1Box.Bottom;
                }
                else
                {
                    centerPoint.Y = obj2Box.Bottom;
                    closestPointObj1.Y = obj1Box.Top;
                }
                if (obj1Midpoint.X < obj2Box.Left)
                {
                    centerPoint.X = obj2Box.Left;
                    closestPointObj1.X = obj1Box.Right;
                }
                else
                {
                    centerPoint.X = obj2Box.Right;
                    closestPointObj1.X = obj1Box.Left;
                }
                
                collisionVector += DiagonalCollision(obj1, new Circle(centerPoint, Globals.cornerRadius), closestPointObj1);
            }
            #endregion
            #endregion
            return collisionVector;
        }
        public abstract bool CollisionCheck(CollisionObject obj);
        #endregion
        #region virtual Get-methods
        public virtual Rectangle GetCollisionBox()
        {
            return Rectangle.Empty;
        }
        public virtual Vector2 GetSpeed()
        {
            return Vector2.Zero;
        }
        public virtual float GetBounciness()
        {
            return 0;
        }
        #endregion

        private static Vector2 DiagonalCollision(CollisionObject obj1, Circle corner, Vector2 startPos)
        {
            #region variables from obj1
            Vector2 speed = obj1.GetSpeed();
            #endregion

            #region Get line from movement vector and position
            Line trajectoryObj = new Line(speed, startPos);
            #endregion
            #region Find where line intersects circle
            Vector2[] intersects = corner.Intersects(trajectoryObj);
            #endregion
            #region Find which one of the intersections is closer
            int closestIndex = GetClosestVector(intersects, startPos);
            if(closestIndex == -1)
            {
                return Vector2.Zero;
            }
            Vector2 closest = intersects[closestIndex];
            #endregion
            #region Define startpoint of vector
            Vector2 startPoint = closest - speed;
            #endregion
            #region Construct a line through the intersection found earlier and the center of the circle
            Line middlePointThroughIntersect = new Line(closest, corner.center);
            #endregion
            #region Construct a line perpandicular to this line through the startpoint
            Line PerpendicularLineThroughStart = new Line(new Vector2(middlePointThroughIntersect.multipliers.Y, -middlePointThroughIntersect.multipliers.X), startPoint);
            #endregion
            #region Find the intersection of the perpandicular with the line through the center of the circle
            Vector2 intersectPerp = PerpendicularLineThroughStart.Intersect(middlePointThroughIntersect);
            #endregion
            #region Find vector from startposition to intersect of perpandicular and center line
            Vector2 diffStartIntersect = intersectPerp - startPoint;
            #endregion
            #region Add vector to the intersection to find the reflected point
            Vector2 endPoint = intersectPerp + diffStartIntersect;
            #endregion
            #region Return vector from intersect of incoming vector's intersect with circle to reflected point
            return endPoint - closest;
            #endregion

            //Illustration in documentation
        }

        private static int GetClosestVector(Vector2[] compare, Vector2 compareTo)
        {
            int currClosest = -1;
            float currClosestDist = float.MaxValue;

            for(int i = 0; i < compare.Length; i++)
            {
                if(Vector2.Distance(compareTo, compare[i]) < currClosestDist)
                {
                    currClosestDist = Vector2.Distance(compareTo, compare[i]);
                    currClosest = i;
                }
            }

            return currClosest;
        }
    }
}