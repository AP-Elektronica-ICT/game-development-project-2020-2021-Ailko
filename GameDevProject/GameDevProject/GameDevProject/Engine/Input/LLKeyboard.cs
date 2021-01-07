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
    public class LLKeyboard : Input
    {
        #region variables
        public KeyboardState newKeyboard, oldKeyboard;
        #endregion

        #region constructors
        public LLKeyboard()
        {
            oldKeyboard = Keyboard.GetState();
            newKeyboard = oldKeyboard;
        }
        #endregion
        #region Input
        public virtual void Update()
        {
            oldKeyboard = newKeyboard;
            newKeyboard = Keyboard.GetState();
        }
        public bool GetPress(string _key)
        {
            for (int i = 0; i < newKeyboard.GetPressedKeyCount(); i++)
            {
                if (newKeyboard.GetPressedKeys()[i].ToString() == _key)
                {
                    return true;
                }
            }
            return false;
        }
        public Action[] ActionsPressed()
        {
            List<Action> actions = new List<Action>();
            if (GetPress("Right"))
                actions.Add(Action.Right);
            if (GetPress("Left"))
                actions.Add(Action.Left);
            if (GetPress("Up") || GetPress("Space"))
                actions.Add(Action.Jump);
            if (GetPress("NumPad0"))
                actions.Add(Action.Shoot);
            return actions.ToArray();
        }
        #endregion
    }
}
