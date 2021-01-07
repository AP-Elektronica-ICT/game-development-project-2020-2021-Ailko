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

namespace GameDevProject.Engine.PhysicsComponents
{
    public static class CollisionEngine
    {
        #region CollisionCheck
        public static bool CollisionCheck(CollisionObject obj1, CollisionObject obj2)
        {
            if (obj1.GetCollisionBox().Intersects(obj2.GetCollisionBox()) || obj1.GetCollisionBox().Contains(obj2.GetCollisionBox()) || obj2.GetCollisionBox().Contains(obj1.GetCollisionBox()))
                return true;
            return false;
        }
        public static bool CollisionCheck(Rectangle obj1, Rectangle obj2)
        {
            if (obj1.Intersects(obj2) || obj1.Contains(obj2) || obj2.Contains(obj1))
                return true;
            return false;
        }
        public static bool CollisionCheck(Rectangle obj, Segment segment)
        {
            if (segment.defPoint1.X > obj.Left && segment.defPoint1.X < obj.Right && segment.defPoint1.Y > obj.Top && segment.defPoint1.Y < obj.Bottom)
                return true;
            if (segment.defPoint2.X > obj.Left && segment.defPoint2.X < obj.Right && segment.defPoint2.Y > obj.Top && segment.defPoint2.Y < obj.Bottom)
                return true;
            Line line = segment.ConvertToLine();
            Vector2[] intersects = line.Intersects(obj);
            for(int i = 0; i < intersects.Length; i++)
            {
                if (segment.Contains(intersects[i]))
                    return true;
            }
            return false;
        }
        public static bool CollisionCheck(CollisionObject[] objects, Segment segment)
        {
            for(int i = 0; i < objects.Length; i++)
            {
                if (CollisionCheck(objects[i].GetCollisionBox(), segment))
                    return true;
            }
            return false;
        }
        public static bool CollisionCheck(CollisionObject obj1, CollisionObject[] objectArr)
        {
            for (int i = 0; i < objectArr.Length; i++)
            {
                if (CollisionCheck(obj1, objectArr[i]))
                    return true;
            }
            return false;
        }
        public static bool CollisionCheck(Rectangle obj1, CollisionObject[] objectArr)
        {
            for (int i = 0; i < objectArr.Length; i++)
            {
                if (CollisionCheck(obj1, objectArr[i].GetCollisionBox()))
                    return true;
            }
            return false;
        }
        #endregion
        public static Vector2 CollisionSide(CollisionObject objRelPos, CollisionObject objColl)
        {
            Vector2 output = Vector2.Zero;

            if (CollisionCheck(objRelPos, objColl))
            {
                Rectangle relPosRect = objRelPos.GetCollisionBox();
                Rectangle collRect = objColl.GetCollisionBox();

                if ((relPosRect.Center.Y < collRect.Bottom) && (relPosRect.Center.Y > collRect.Top))
                {
                    if (relPosRect.Center.X > collRect.Left && relPosRect.Center.X < collRect.Right)
                    {
                        if (relPosRect.Center.Y < collRect.Center.Y)
                        {
                            output.Y = -1;
                        }
                        else if ( relPosRect.Center.Y > collRect.Center.Y )
                        {
                            output.Y = 1;
                        }

                        if (relPosRect.Center.X < collRect.Center.X)
                        {
                            output.X = -1;
                        }
                        else if (relPosRect.Center.X > collRect.Center.X)
                        {
                            output.X = 1;
                        }
                    }
                    else
                    {
                        if (relPosRect.Center.X < collRect.Left)
                        {
                            output.X = -1;
                        }
                        else
                        {
                            output.X = 1;
                        }
                    }
                }
                else
                {
                    if(relPosRect.Center.Y < collRect.Top)
                    {
                        output.Y = -1;
                    }
                    else
                    {
                        output.Y = 1;
                    }
                }
            }

            return output;
        }
    }
}
