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
    public class LLMouse : Input
    {
        #region variables
        public MouseState newMouse, oldMouse;
        #endregion

        #region constructors
        public LLMouse()
        {
            newMouse = Mouse.GetState();
            oldMouse = newMouse;
        }
        #endregion
        #region Input
        public void Update()
        {
            oldMouse = newMouse;
            newMouse = Mouse.GetState();
        }
        public Action[] ActionsPressed()
        {
            List<Action> actions = new List<Action>();
            if (newMouse.LeftButton == ButtonState.Pressed && oldMouse.LeftButton == ButtonState.Released)
                actions.Add(Action.Click);
            return actions.ToArray();
        }
        public Vector2 PointerPos()
        {
            return new Vector2(newMouse.Position.X, newMouse.Position.Y);
        }
        #endregion
    }
}