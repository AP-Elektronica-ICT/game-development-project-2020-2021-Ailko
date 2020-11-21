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
    public class LLKeyboard
    {
        public KeyboardState newKeyboard, oldKeyboard;

        public LLKeyboard()
        {

        }

        public virtual void Update()
        {
            oldKeyboard = newKeyboard;
            newKeyboard = Keyboard.GetState();
        }

        public bool GetPress(string _key)
        {
            for(int i = 0; i < newKeyboard.GetPressedKeyCount(); i++)
            {
                if(newKeyboard.GetPressedKeys()[i].ToString() == _key)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
