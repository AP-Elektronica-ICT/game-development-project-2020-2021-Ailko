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
using GameDevProject.Engine.Interfaces;
using GameDevProject.Engine.Geometry;
using GameDevProject.Engine.PhysicsComponents;
using GameDevProject.Engine.Gamestructure.WorldObjects;
using GameDevProject.Engine.Gamestructure.WorldObjects.Entities;

namespace GameDevProject.Engine
{
    public static class PhysicsEngine
    {
        /// <summary>
        /// Goes through all the CollisionObjects in the current level and runs all the physics methods on these CollisionObjects.
        /// </summary>
        public static void Update()
        {
            CollisionObject[] objects = Globals.currWorld.levels[Globals.currWorld.currLevel].CollisionObjects();
            for (int i = 0; i < objects.Length; i++)
            {
                if (!objects[i].IsStatic())
                {
                    objects[i].SavePhysicsVector(Gravity(objects[i]));
                    objects[i].SavePhysicsVector(DragVector(objects[i]));
                    objects[i].SavePhysicsVector(CollisionForces(objects[i], objects));
                }
            }
        }
        #region collision
        /// <summary>
        /// Calculates all the collisionforces acting on a CollisionObject.
        /// </summary>
        /// <param name="obj">The CollisionObject on which the collisions act.</param>
        /// <param name="objects">The other CollisionObjects which act on obj.</param>
        /// <returns>The sum of all the collision forces.</returns>
        public static Vector2 CollisionForces(CollisionObject obj, CollisionObject[] objects)
        {
            Vector2 output = Vector2.Zero;
            for (int j = 0; j < objects.Length; j++)
            {
                if (obj != objects[j])
                {
                    Vector2 collision = CollisionEngine.CollisionSide(obj, objects[j]);
                    if (collision != Vector2.Zero)
                    {
                        output += CollisionForce(obj, objects[j], collision);
                    }
                }
            }
            return output;
        }
        /// <summary>
        /// Calculates the collision vector between 2 CollisionObjects as seen from refObj, given a certain collision direction.
        /// </summary>
        /// <param name="refObj">The object which collides with the other.</param>
        /// <param name="collObj">The object with which the first collides.</param>
        /// <param name="collisionDir">The directionality of the collision.</param>
        /// <returns>A vector representing the normal force collObj enacts on refObj.</returns>
        public static Vector2 CollisionForce(CollisionObject refObj, CollisionObject collObj, Vector2 collisionDir)
        {
            #region creation variables
            Vector2 collisionVector = Vector2.Zero;

            Rectangle refBox = refObj.GetCollisionBox();
            Rectangle collBox = collObj.GetCollisionBox();

            float smallestWidth;
            float smallestHeight;
            if (refBox.Width < collBox.Width)
                smallestWidth = refBox.Width;
            else
                smallestWidth = collBox.Width;

            if (refBox.Height < collBox.Height)
                smallestHeight = refBox.Height;
            else
                smallestHeight = collBox.Height;
            #endregion
            #region collision
            switch(collisionDir.ToString())
            {
                #region vertical
                case "{X:0 Y:-1}":
                    if(refObj.GetSpeedTotal().Y > 0)
                        collisionVector.Y -= refObj.GetSpeedTotal().Y * (1 + (collObj.GetBounciness() / 10) + Math.Abs(refBox.Bottom - collBox.Top) / smallestHeight);
                    break;
                case "{X:0 Y:1}":
                    if(refObj.GetSpeedTotal().Y < 0)
                        collisionVector.Y -= refObj.GetSpeedTotal().Y * (1 + (collObj.GetBounciness() / 10) + Math.Abs(collBox.Bottom - refBox.Top) / smallestHeight);
                    break;
                #endregion
                #region horizontal
                case "{X:-1 Y:0}":
                    if(refObj.GetSpeedTotal().X > 0 && (refBox.Center.Y + (refBox.Height * 0.9f) / 2 > collBox.Top || refBox.Center.Y - (refBox.Height * 0.9f) / 2 < collBox.Bottom))
                    {
                        collisionVector.X -= refObj.GetSpeedTotal().X * (1 + (collObj.GetBounciness() / 10) + Math.Abs(collBox.Left - refBox.Right) / (smallestWidth));
                    }
                    if (Globals.input.ActionsPressed().Contains(Input.Action.Right))
                        collisionVector.X -= 0.1f;
                    break;
                case "{X:1 Y:0}":
                    if(refObj.GetSpeedTotal().X < 0 && (refBox.Center.Y + (refBox.Height * 0.9f) / 2 > collBox.Top || refBox.Center.Y - (refBox.Height * 0.9f) / 2 < collBox.Bottom))
                    {
                        collisionVector.X -= refObj.GetSpeedTotal().X * (1 + (collObj.GetBounciness() / 10) + Math.Abs(refBox.Left - collBox.Right) / (smallestWidth));
                    }
                    if (Globals.input.ActionsPressed().Contains(Input.Action.Left))
                        collisionVector.X += 0.1f;
                    break;
                #endregion
                #region diagonal
                case "{X:-1 Y:-1}":
                case "{X:-1 Y:1}":
                case "{X:1 Y:-1}":
                case "{X:1 Y:1}":
                    Vector2 center = new Vector2(collBox.Center.X, collBox.Center.Y);
                    collisionVector += DiagonalCollision(refObj, new Circle(center, Vector2.Distance(center, new Vector2(collBox.X, collBox.Y)))) * PhysicsConstants.metersPerPixel;
                    break;
                #endregion
            }
            if (Math.Sign(collObj.GetSpeedTotal().X) != Math.Sign(refObj.GetSpeedTotal().X))
                collisionVector += collObj.GetSpeed() * refObj.CollisionImpactMultiplier();
            #endregion
            return collisionVector;
        }
        /// <summary>
        /// Returns the collisionvector of a corner collision calculated with a circle in the center of the collision box.
        /// </summary>
        /// <param name="obj1">The object colliding with the corner.</param>
        /// <param name="corner">The circle for this corner.</param>
        /// <returns>A Vector2 of the collision.</returns>
        public static Vector2 DiagonalCollision(CollisionObject obj1, Circle corner)
        {
            #region variables from obj1
            Vector2 speed = obj1.GetSpeedTotal();
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
            if (closestIndex == -1)
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
        /// <summary>
        /// Finds the closest Vector2 out of a list to a certain Vector2.
        /// </summary>
        /// <param name="compare">The list of vectors to find the closest one in.</param>
        /// <param name="compareTo">The point to which the method finds the closest vector.</param>
        /// <returns>The integer of the closest Vector2 in the array.</returns>
        public static int GetClosestVector(Vector2[] compare, Vector2 compareTo)
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
        #endregion
        #region drag
        /// <summary>
        /// Determines the drag vector on a CollisionObject.
        /// </summary>
        /// <param name="obj">The CollisionObject.</param>
        /// <returns>A vector representing the drag-force.</returns>
        public static Vector2 DragVector(CollisionObject obj)
        {
            float dragY = -0.5f * PhysicsConstants.airDensity * PhysicsConstants.dragCoeff * (float)Math.Pow(obj.GetCollisionBox().Width * PhysicsConstants.metersPerPixel, 2) * (float)Math.Pow(obj.GetSpeed().Y, 3) / Math.Abs(obj.GetSpeed().Y);
            float dragX = -0.5f * PhysicsConstants.airDensity * PhysicsConstants.dragCoeff * (float)Math.Pow(obj.GetCollisionBox().Height * PhysicsConstants.metersPerPixel, 2) * (float)Math.Pow(obj.GetSpeed().X, 3) / Math.Abs(obj.GetSpeed().X);
            #region check for NaN
            if (float.IsNaN(dragY))
                dragY = 0;
            if (float.IsNaN(dragX))
                dragX = 0;
            #endregion
            return new Vector2(dragX / obj.GetMass(), dragY / obj.GetMass()) * (float)Globals.deltaTime.TotalSeconds * PhysicsConstants.metersPerPixel;
        }
        #endregion
        #region gravity
        /// <summary>
        /// Determines the gravity force on a CollisionObject.
        /// </summary>
        /// <param name="obj">The object to determine the gravity-force for.</param>
        /// <returns>A vector representing the gravitational force.</returns>
        public static Vector2 Gravity(CollisionObject obj)
        {
            return new Vector2(0, PhysicsConstants.gravity * obj.GetMass()) * (float)Globals.deltaTime.TotalSeconds * PhysicsConstants.metersPerPixel;
        }
        #endregion
    }
}
