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

namespace GameDevProject.Engine.Interfaces
{
    enum collisionAngle { Horizontale, Vertical, Diagonal, None, Inside }
    public interface CollisionObject
    {
        public abstract void SavePhysicsVector(Vector2 vector);
        #region virtual Get-methods
        public virtual Rectangle GetCollisionBox()
        {
            return Rectangle.Empty;
        }
        public virtual Vector2 GetSpeed()
        {
            return Vector2.Zero;
        }
        public virtual Vector2 GetSpeedTotal()
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
        public virtual bool IsStatic()
        {
            return true;
        }
        public virtual float GetMass()
        {
            return 0;
        }
        public virtual float CollisionImpactMultiplier()
        {
            return 0;
        }
        #endregion
    }
}
