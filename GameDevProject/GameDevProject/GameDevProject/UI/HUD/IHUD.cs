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

namespace GameDevProject.UI.HUD
{
    public enum Outlining { Left, Center, Right }
    public abstract class IHUD
    {
        #region variables
        public Outlining outlining;
        public string text;
        public Vector2 pos;
        #endregion

        #region constructors
        public IHUD(Vector2 _pos, string _text = "", Outlining _outlining = Outlining.Left)
        {
            text = _text;
            pos = _pos;
            outlining = _outlining;
        }
        #endregion
        public abstract void Update();
        #region draws
        public virtual void Draw()
        {
            switch (outlining)
            {
                case Outlining.Left:
                    Globals.spriteBatch2.DrawString(Globals.arial, text, pos, Color.White);
                    break;
                case Outlining.Center:
                    Globals.spriteBatch2.DrawString(Globals.arial, text, pos - new Vector2((int)Math.Floor(text.Length / 2.0), 0), Color.White);
                    break;
                case Outlining.Right:
                    Globals.spriteBatch2.DrawString(Globals.arial, text, pos - new Vector2(text.Length, 0), Color.White);
                    break;
            }
            Globals.spriteBatch2.DrawString(Globals.arial, text, pos, Color.White);
        }
        public virtual void Draw(string _text, Color color)
        {
            switch (outlining)
            {
                case Outlining.Left:
                    Globals.spriteBatch2.DrawString(Globals.arial, _text, pos, color);
                    break;
                case Outlining.Center:
                    Globals.spriteBatch2.DrawString(Globals.arial, _text, pos - new Vector2((int)Math.Floor(_text.Length / 2.0), 0), color);
                    break;
                case Outlining.Right:
                    Globals.spriteBatch2.DrawString(Globals.arial, _text, pos - new Vector2(_text.Length, 0), color);
                    break;
            }
        }
        public virtual void Draw(string _text, Vector2 posOffset, Color color)
        {
            Globals.spriteBatch2.DrawString(Globals.arial, _text, pos + posOffset, color);
        }
        #endregion
    }
}
