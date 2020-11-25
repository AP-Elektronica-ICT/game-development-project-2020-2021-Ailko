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
    public class Sprite
    {
        #region variables
        public Vector2 pos, dims;
        public Texture2D sprite;
        #endregion

        public Sprite(string _path, Vector2 _pos, Vector2 _dims)
        {
            pos = _pos;
            dims = _dims;
            if(_path != "")
                sprite = Globals.content.Load<Texture2D>(_path);
        }

        public virtual void Update()
        {

        }

        #region draw methods
        public virtual void Draw()
        {
            if(sprite != null)
            {
                Globals.spriteBatch.Draw
                    (
                        sprite,
                        new Rectangle
                        (
                            (int)pos.X,
                            (int)pos.Y,
                            (int)dims.X,
                            (int)dims.Y
                        ),
                        null,
                        Color.White,
                        0,
                        Vector2.Zero,
                        SpriteEffects.None,
                        0
                    );
            }
        }
        public virtual void Draw(Rectangle frame, Vector2 dimChange, SpriteEffects effect = SpriteEffects.None)
        {
            if (sprite != null)
            {
                Globals.spriteBatch.Draw
                    (
                        sprite,
                        new Rectangle
                        (
                            (int)pos.X,
                            (int)pos.Y,
                            (int)(dims.X + dimChange.X),
                            (int)(dims.Y + dimChange.Y)
                        ),
                        frame,
                        Color.White,
                        0,
                        Vector2.Zero,
                        effect,
                        0
                    );
            }
        }
        public virtual void Draw(Rectangle frame, Vector2 dimChange, Vector2 posChange,  SpriteEffects effect = SpriteEffects.None)
        {
            if (sprite != null)
            {
                Globals.spriteBatch.Draw
                    (
                        sprite,
                        new Rectangle
                        (
                            (int)(pos.X + posChange.X),
                            (int)(pos.Y + posChange.Y),
                            (int)(dims.X + dimChange.X),
                            (int)(dims.Y + dimChange.Y)
                        ),
                        frame,
                        Color.White,
                        0,
                        Vector2.Zero,
                        effect,
                        0
                    );
            }
        }
        #endregion
    }
}
