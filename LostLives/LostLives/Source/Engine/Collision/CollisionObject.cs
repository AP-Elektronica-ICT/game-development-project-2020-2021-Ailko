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
    enum collisionAngle { Horizontale, Vertical, Diagonal, None, Inside }
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
            if ((obj1Midpoint.X < obj2Box.Left - obj1Box.Width / 2 || obj1Midpoint.X > obj2Box.Right + obj1Box.Width / 2) || (obj1Midpoint.Y < obj2Box.Bottom && obj1Midpoint.Y > obj2Box.Top))
            {
                if (obj1Midpoint.Y < obj2Box.Top || obj1Midpoint.Y > obj2Box.Bottom)
                {
                    if ((obj1Midpoint.X > obj2Box.Left && obj1Midpoint.X < obj2Box.Right) || (obj1Midpoint.Y > obj2Box.Top && obj1Midpoint.Y < obj2Box.Bottom))
                        angle = collisionAngle.Inside;
                    else
                        angle = collisionAngle.Diagonal;
                }
                else
                {
                    angle = collisionAngle.Horizontale;
                }
            }
            else
            {
                angle = collisionAngle.Vertical;
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
                Vector2 center = new Vector2((obj2Box.Right - obj2Box.Left) / 2, (obj2Box.Bottom - obj2Box.Top) / 2);
                collisionVector += DiagonalCollision(obj1, new Circle(center, Vector2.Distance(center, new Vector2(obj2Box.X, obj2Box.Y)))) * Globals.metersPerPixel/* (float)Globals.deltaTime.TotalSeconds*/;
            }
            #endregion
            #region inside collision
            if(angle == collisionAngle.Inside)
            {
                Vector2 vectMidPlayerToMid = obj2.GetCenter() - obj1Midpoint;
                Line line = new Line(obj1Midpoint, obj2.GetCenter());
                List<Vector2> intersects = new List<Vector2>();
                {
                    Vector2 temp = Vector2.Zero;
                    temp = line.whereX(obj2Box.Left);
                    if (temp.Y <= obj2Box.Bottom && temp.Y >= obj2Box.Top)
                        intersects.Add(new Vector2(temp.X, temp.Y));
                    temp = line.whereX(obj2Box.Right);
                    if (temp.Y <= obj2Box.Bottom && temp.Y >= obj2Box.Top)
                        intersects.Add(new Vector2(temp.X, temp.Y));
                    temp = line.whereY(obj2Box.Bottom);
                    if (temp.X <= obj2Box.Right && temp.X >= obj2Box.Left)
                        intersects.Add(new Vector2(temp.X, temp.Y));
                    temp = line.whereY(obj2Box.Top);
                    if (temp.X <= obj2Box.Right && temp.X >= obj2Box.Left)
                        intersects.Add(new Vector2(temp.X, temp.Y));
                }
                int closest = GetClosestVector(intersects.ToArray(), obj1Midpoint);
                Vector2 closestVect = intersects[closest];
                Vector2 resultVect = (obj2.GetCenter() - closestVect) - vectMidPlayerToMid;
                collisionVector += -resultVect;
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
        public virtual Vector2 GetCenter()
        {
            Rectangle collBox = GetCollisionBox();
            return new Vector2((collBox.Right - collBox.Left) / 2, (collBox.Bottom - collBox.Top) / 2);
        }
        #endregion

        private static Vector2 DiagonalCollision(CollisionObject obj1, Circle corner)
        {
            #region variables from obj1
            Vector2 speed = obj1.GetSpeed();
            Vector2 startPos = obj1.GetCenter();
            #endregion
            
            #region Get line from movement vector and position
            Line trajectoryObj = new Line(startPos, startPos + speed);
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
            Line PerpendicularLineThroughStart = new Line(new Vector2(middlePointThroughIntersect.twoPointFormY, -middlePointThroughIntersect.twoPointFormX), startPoint);
            #endregion
            #region Find the intersection of the perpandicular with the line through the center of the circle
            Vector2 intersectPerp = PerpendicularLineThroughStart.Intersects(middlePointThroughIntersect);
            #endregion
            #region Find vector from startposition to intersect of perpandicular and center line
            Vector2 diffStartIntersect = intersectPerp - startPoint;
            #endregion
            #region Add vector to the intersection to find the reflected point
            Vector2 endPoint = intersectPerp + diffStartIntersect;
            #endregion
            #region Return vector from intersect of incoming vector's intersect with circle to reflected point
            return -(endPoint - closest) - speed;
            #endregion

            //Illustration in documentation
        }

        private static int GetClosestVector(Vector2[] compare, Vector2 compareTo)
        {
            if (compare != null)
            {
                int currClosest = -1;
                float currClosestDist = float.MaxValue;

                for (int i = 0; i < compare.Length; i++)
                {
                    if (Vector2.Distance(compareTo, compare[i]) < currClosestDist)
                    {
                        currClosestDist = Vector2.Distance(compareTo, compare[i]);
                        currClosest = i;
                    }
                }

                return currClosest;
            }
            return -1;
        }
    }
}