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
    public interface Entity : CollisionObject
    {
        public abstract void Update();
        public abstract void Move();
        public virtual void ComputeInput() { }
        public abstract void UpdateFrame();
        public abstract void Draw();
        public abstract Entity Clone();
        #region battle
        public abstract void Attack();
        public abstract void GetHit(float _damage);
        public abstract void Die();
        #endregion
        #region CollisionObject
        new public virtual bool IsStatic()
        {
            return false;
        }
        #endregion
    }
}
