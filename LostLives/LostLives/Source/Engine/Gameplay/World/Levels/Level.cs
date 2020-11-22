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
    public class Level : Sprite
    {
        public Platform[] platforms;
        public Texture2D background;
        public Level(Platform[] _platforms, string _path = "") : base(_path, Vector2.Zero, Globals.screenSize)
        {
            platforms = _platforms;
            if(_path != "")
            {
                background = Globals.content.Load<Texture2D>(_path);
            }
        }

        public int Update(CollisionObject obj)
        {
            for(int i = 0; i < platforms.Length; i++)
            {
                if(platforms[i].CollisionCheck(obj))
                {
                    return i;
                }
            }
            return -1;
        }

        public override void Draw()
        {
            //base.Draw();
            foreach(Platform platform in platforms)
            {
                platform.Draw();
            }
        }
    }
}
