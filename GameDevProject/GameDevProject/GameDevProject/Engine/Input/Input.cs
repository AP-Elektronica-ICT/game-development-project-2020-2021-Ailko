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

namespace GameDevProject.Engine.Input
{
    public enum Action { Right, Left, Jump, Shoot, Click }
    public interface Input
    {
        public abstract Action[] ActionsPressed();
        public abstract void Update();
        public virtual Vector2 PointerPos()
        {
            return Vector2.Zero;
        }
    }
}