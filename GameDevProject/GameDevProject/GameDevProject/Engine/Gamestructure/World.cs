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
using GameDevProject.Engine.Interfaces;
using GameDevProject.Engine.Gamestructure.WorldObjects.Entities;

namespace GameDevProject.Engine.Gamestructure
{
    public class World
    {
        #region variables
        public Hero hero;
        public Level[] levels;
        public int currLevel = 0;
        #endregion

        #region constructors
        public World()
        {
            hero = new Hero(new Vector2(0, 0), new Vector2(41, 54));
            levels = new Level[]
            {
                new Level
                (
                    new Rectangle[]
                    {
                        new Rectangle(-11, -10, 13, 30),
                        new Rectangle(2, -10, 100, 12),
                        new Rectangle(2, 12, 100, 10),
                        new Rectangle(12, -10, 13, 30)
                    },
                    new Entity[]
                    {
                        hero
                    },
                    new Vector2(64, 330),
                    60 * 32
                ),
                new Level
                (
                    new Rectangle[]
                    {
                        new Rectangle(-11, -10, 13, 30),
                        new Rectangle(2, -10, 100, 12),
                        new Rectangle(2, 12, 100, 10),
                        new Rectangle(6, -2, 4, 6),
                        new Rectangle(7, 10, 5, 2),
                        new Rectangle(16, 8, 4, 2),
                        new Rectangle(21, 6, 3, 2),
                        new Rectangle(22, 10, 5, 2),
                        new Rectangle(30, 6, 8, 2),
                        new Rectangle(45, 7, 8, 2),
                        new Rectangle(50, 9, 3, 3)
                    },
                    new Entity[]
                    {
                        hero,
                        new Imp(new Vector2(23 * 32, 9 * 32 - 53), new Vector2(39, 53)),
                        new Imp(new Vector2(32 * 32, 5 * 32 - 53), new Vector2(39, 53)),
                        new Imp(new Vector2(45 * 32, 12 * 32 - 53), new Vector2(39, 53)),
                        new Imp(new Vector2(47 * 32, 12 * 32 - 53), new Vector2(39, 53))
                    },
                    new Vector2(64, 330),
                    60 * 32
                ),
                new Level
                (
                    new Rectangle[]
                    {
                        new Rectangle(-11, -21, 13, 44),
                        new Rectangle(2, -20, 100, 12),
                        new Rectangle(2, 12, 100, 10),
                        new Rectangle(6, 10, 4, 2),
                        new Rectangle(14, 8, 2, 2),
                        new Rectangle(7, 6, 2, 2),
                        new Rectangle(2, 4, 2, 2),
                        new Rectangle(9, 2, 2, 2),
                        new Rectangle(16, 0, 7, 2),
                        new Rectangle(23, 0, 3, 12),
                        new Rectangle(24, -6, 100, 2),
                        new Rectangle(26, 2, 3, 2),
                        new Rectangle(34, 6, 3, 2),
                        new Rectangle(42, 10, 3, 2)
                    },
                    new Entity[]
                    {
                        hero,
                        new Imp(new Vector2(40 * 32, 12 * 32 - 53), new Vector2(39, 53)),
                        new Imp(new Vector2(38 * 32, 12 * 32 - 53), new Vector2(39, 53)),
                        new Imp(new Vector2(32 * 32, 12 * 32 - 53), new Vector2(39, 53)),
                        new Imp(new Vector2(34 * 32, 12 * 32 - 53), new Vector2(39, 53)),
                        new Imp(new Vector2(34 * 32, 6 * 32 - 53), new Vector2(39, 53))
                    },
                    new Vector2(64, 330),
                    50 * 32
                ),
                new Level
                (
                    new Rectangle[]
                    {
                        new Rectangle(-11, -10, 13, 30),
                        new Rectangle(2, -10, 100, 12),
                        new Rectangle(2, 12, 100, 10),
                        new Rectangle(12, -10, 13, 30)
                    },
                    new Entity[]
                    {
                        hero
                    },
                    new Vector2(64, 330),
                    60 * 32
                )
            };
            levels[currLevel].StartLevel();
        }
        #endregion
        #region methods
        public void Update()
        {
            Globals.deltaTime = DateTime.Now - Globals.lastFrame;
            levels[currLevel].Update();
            Globals.lastFrame = DateTime.Now;
        }
        public void Draw()
        {
            levels[currLevel].Draw();
        }
        #endregion
    }
}