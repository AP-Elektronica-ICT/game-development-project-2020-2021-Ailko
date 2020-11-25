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
    public class World
    {
        public Sprite hero;
        public Levels levels;

        public World()
        {
            hero = new Hero("Sprites\\DoomGuy", new Vector2(64, 330), new Vector2(41, 54));
            levels = new Levels
            (
                new Level[]
                {
                    new Level
                    (
                        new Rectangle[]
                        {
                            new Rectangle(-1, -2, 2, 19),
                            new Rectangle(1, 12, 20, 4),
                            new Rectangle(13, 8, 5, 2),
                            new Rectangle(7, 10, 3, 2),
                            new Rectangle(1, -1, 20, 2),
                            new Rectangle(6, 6, 3, 2)
                        }
                    )
                }
            );
        }

        public virtual void Update()
        {
            Globals.deltaTime = DateTime.Now - Globals.lastFrame;

            hero.Update();

            Globals.lastFrame = DateTime.Now;
        }

        public virtual void Draw()
        {
            hero.Draw();
            levels.Draw();
        }
    }
}
