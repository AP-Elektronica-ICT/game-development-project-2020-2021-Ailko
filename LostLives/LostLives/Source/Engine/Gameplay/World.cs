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
            hero = new Hero("Sprites\\DoomGuy", new Vector2(64, 200), new Vector2(41, 54));
            levels = new Levels
            (
                new Level[]
                {
                    new Level
                    (
                        new Platform[]
                        {
                            new Platform
                            (
                                1,
                                17,
                                new Vector2(0, 0),
                                _rightIntersectPos: "384,448"
                            ),
                            new Platform
                            (
                                20,
                                2,
                                new Vector2(32, 384),
                                WallAdjacent.Left
                            ),

                            new Platform
                            (
                                5,
                                1,
                                new Vector2(420, 288)
                            )
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
