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
        public abstract bool CollisionCheck(CollisionObject obj);
        public static Vector2 CollisionSpecifics(CollisionObject obj1, CollisionObject obj2)
        {
            Vector2 collisionVector = Vector2.Zero;

            Rectangle obj1Box = obj1.GetCollisionBox();
            Rectangle obj2Box = obj2.GetCollisionBox();

            float difY = obj2Box.Y - obj1Box.Y;
            float difX = 0;
            if (obj1Box.X < obj2Box.X)
                difX = obj2Box.X - obj1Box.X;
            else if (obj1Box.X + obj1Box.Width > obj2Box.X + obj2Box.Width)
                difX = (obj1Box.X + obj1Box.Width) - (obj2Box.X + obj2Box.Width);

            /*if (difY > 5 + obj1Box.Height)
            {
                collisionVector.Y -= (float)(Math.Pow(obj1.GetSpeed().Y, obj2.GetBounciness()) * (1 + (obj1Box.Height - difY)  / obj1Box.Height));
            }
            else if (difY < -5)
            {
                collisionVector.Y += (float)(Math.Pow(obj1.GetSpeed().Y, obj2.GetBounciness()) * (-difY / obj2Box.Height));
            }
            else if(difY > 0)
            {
                collisionVector.Y += -Globals.gravity * (float)Globals.deltaTime.TotalSeconds * (1 + 1/difY) + 0.5f * Globals.airDensity * 0.7f * 0.2f * (float)Math.Pow(obj1.GetSpeed().Y, 3) / Math.Abs(obj1.GetSpeed().Y); ;
            }
            else
            {
                collisionVector.Y += -obj1.GetSpeed().Y;
            }*/
            if (Math.Abs(difY) > Math.Abs(difX))
                collisionVector.Y -= obj1.GetSpeed().Y * (1 + (obj2.GetBounciness() / 10));
            if(Math.Abs(difX) > Math.Abs(difY))
                collisionVector.X -= obj1.GetSpeed().X * (1 + (obj2.GetBounciness() / 10));

            return collisionVector;
        }
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
    }
}
