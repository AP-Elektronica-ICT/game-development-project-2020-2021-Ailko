﻿#region Includes
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
            hero = new Hero("Sprites\\DoomGuy", new Vector2(0, 200), new Vector2(41, 54));
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
                                20,
                                new Vector2(0, 467),
                                _trailOff: TrailOffs.Right
                            ),

                            new Platform
                            (
                                5,
                                new Vector2(420, 350),
                                _trailOff: TrailOffs.Both
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
