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

namespace GameDevProject.UI.Interfaces
{
    public class Button
    {
        #region variables
        public Executable exe;
        public string text;
        public Color colorNormal;
        public Color colorHover;
        public Color currColor;
        public float scale;
        public Rectangle hitBox;
        #endregion

        #region constructors
        public Button(Rectangle _hitBox, Executable _exe, Color _colorNormal, Color _colorHover, string _text = "", float _scale = 1)
        {
            hitBox = _hitBox;
            exe = _exe;
            text = _text;
            colorNormal = _colorNormal;
            colorHover = _colorHover;
            currColor = colorNormal;
            scale = _scale;
        }
        #endregion
        public void Draw()
        {
            Globals.spriteBatch2.DrawString(Globals.arial, text, new Vector2(hitBox.X, hitBox.Y), currColor, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
        }
        public void Update()
        {
            if (hitBox.Contains(Globals.input.PointerPos()))
                currColor = colorHover;
            else
                currColor = colorNormal;
            if (Globals.input.ActionsPressed().Contains(Engine.Input.Action.Click) && currColor == colorHover)
                Execute();
        }
        public void Execute()
        {
            exe.Execute();
        }
    }
}
