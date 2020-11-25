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
    public interface CollisionObject
    {
        #region collision methods
        public static Vector2 CollisionSpecifics(CollisionObject obj1, CollisionObject obj2)
        {
            #region creation variables
            Vector2 collisionVector = Vector2.Zero;

            Rectangle obj1Box = obj1.GetCollisionBox();
            Rectangle obj2Box = obj2.GetCollisionBox();

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
            #region collision
            #region vertical collision
            if (obj1Box.Bottom > obj2Box.Top && obj1Box.Top < obj2Box.Top && !(obj1Box.Left + obj1Box.Width / 2 <= obj2Box.Left || obj1Box.Right - obj1Box.Width / 2 >= obj2Box.Right))
            {
                if (obj1.GetSpeed().Y > 0)
                    collisionVector.Y -= obj1.GetSpeed().Y * (1 + (obj2.GetBounciness() / 10)) * (float)Math.Pow(1 + ((obj1Box.Bottom - obj2Box.Top) / smallestHeight), 2);
            }
            else if (obj1Box.Top < obj2Box.Bottom && obj1Box.Bottom > obj2Box.Bottom && !(obj1Box.Left + obj1Box.Width / 2 <= obj2Box.Left || obj1Box.Right - obj1Box.Width / 2 >= obj2Box.Right))
            {
                if (obj1.GetSpeed().Y < 0)
                    collisionVector.Y -= obj1.GetSpeed().Y * (1 + (obj2.GetBounciness() / 10)) * (float)Math.Pow(1 + ((obj1Box.Top - obj2Box.Bottom) / smallestHeight), 2);
            }
            #endregion
            #region horizontal collision
            if (obj1Box.Right > obj2Box.Left && obj1Box.Left < obj2Box.Left && !(obj1Box.Top + obj1Box.Height / 2 <= obj2Box.Top || obj1Box.Bottom - obj1Box.Height / 2 >= obj2Box.Bottom))
            {
                if (obj1.GetSpeed().X > 0)
                    collisionVector.X -= obj1.GetSpeed().X * (1 + (obj2.GetBounciness() / 10)) * (float)Math.Pow(1 + ((obj1Box.Right - obj2Box.Left) / smallestWidth), 2);
            }
            else if (obj1Box.Left < obj2Box.Right && obj1Box.Right > obj2Box.Right && !(obj1Box.Top + obj1Box.Height / 2 <= obj2Box.Top || obj1Box.Bottom - obj1Box.Height / 2 >= obj2Box.Bottom))
            {
                if (obj1.GetSpeed().X < 0)
                    collisionVector.X -= obj1.GetSpeed().X * (1 + (obj2.GetBounciness() / 10)) * (float)Math.Pow(1 + ((obj1Box.Left - obj2Box.Right) / smallestWidth), 2);
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
    }
}