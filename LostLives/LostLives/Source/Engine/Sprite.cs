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
        public Vector2 pos, dims;
        public Texture2D sprite;

        public Sprite(string _path, Vector2 _pos, Vector2 _dims)
        {
            pos = _pos;
            dims = _dims;
            sprite = Globals.content.Load<Texture2D>(_path);
        }

        public virtual void Update()
        {

        }

        public virtual void Draw()
        {
            if(sprite != null)
            {
                Globals.spriteBatch.Draw(sprite, new Rectangle((int)pos.X, (int)pos.Y, (int)dims.X, (int)dims.Y), null, Color.White, 0.0f, new Vector2(sprite.Bounds.Width / 2, sprite.Bounds.Height / 2), new SpriteEffects(), 0);
            }
        }
    }
}
