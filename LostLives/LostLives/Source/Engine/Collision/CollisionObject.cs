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

            if(obj1Box.Bottom > obj2Box.Top && obj1Box.Top < obj2Box.Top)
            {
                if(obj1.GetSpeed().Y > 0)
                    collisionVector.Y -= obj1.GetSpeed().Y * (1 + (obj2.GetBounciness() / 10) + (obj1Box.Bottom - obj2Box.Top) / 10);
            }
            else if(obj1Box.Top < obj2Box.Bottom && obj1Box.Bottom > obj2Box.Bottom)
            {
                if (obj1.GetSpeed().Y < 0)
                    collisionVector.Y -= obj1.GetSpeed().Y * (1 + (obj2.GetBounciness() / 10) + (obj1Box.Top - obj2Box.Bottom) / 10);
            }

            if(obj1Box.Right > obj2Box.Left && obj1Box.Left < obj2Box.Left && obj1Box.Top >= obj2Box.Top)
            {
                if(obj1.GetSpeed().X > 0)
                    collisionVector.X -= obj1.GetSpeed().X * (1 + (obj2.GetBounciness() / 10) + (obj1Box.Right - obj2Box.Left) / 10);
            }
            else if (obj1Box.Left < obj2Box.Right && obj1Box.Right > obj2Box.Right && obj1Box.Top >= obj2Box.Top)
            {
                if (obj1.GetSpeed().X < 0)
                    collisionVector.X -= obj1.GetSpeed().X * (1 + (obj2.GetBounciness() / 10) + (obj1Box.Left - obj2Box.Right) / 10);
            }

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
