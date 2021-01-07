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
    public class LLMouseAndKeyboard : Input
    {
        #region variables
        LLMouse mouse;
        LLKeyboard keyboard;
        #endregion

        #region constructors
        public LLMouseAndKeyboard()
        {
            mouse = new LLMouse();
            keyboard = new LLKeyboard();
        }
        #endregion
        #region Input
        public void Update()
        {
            mouse.Update();
            keyboard.Update();
        }
        public Action[] ActionsPressed()
        {
            List<Action> actions = new List<Action>();
            actions.AddRange(mouse.ActionsPressed());
            actions.AddRange(keyboard.ActionsPressed());
            return actions.ToArray();
        }
        public Vector2 PointerPos()
        {
            return mouse.PointerPos();
        }
        #endregion
    }
}
