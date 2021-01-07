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

namespace GameDevProject.UI
{
    public class Menu
    {
        #region variables
        float topTextScale;
        Color color;
        Vector2 pos;
        public string topText;
        public Interfaces.Button[] buttons;
        #endregion

        #region constructors
        public Menu(Interfaces.Button[] _buttons, string _topText, Color _color, Vector2 _pos, float _topTextScale = 1)
        {
            pos = _pos;
            color = _color;
            topTextScale = _topTextScale;
            topText = _topText;
            buttons = _buttons;
        }
        #endregion
        public virtual void Update()
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].Update();
            }
        }
        public virtual void Draw()
        {
            Globals.spriteBatch2.DrawString(Globals.arial, topText, pos, color, 0, Vector2.Zero, topTextScale, SpriteEffects.None, 0);
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].Draw();
            }
        }
    }
}
