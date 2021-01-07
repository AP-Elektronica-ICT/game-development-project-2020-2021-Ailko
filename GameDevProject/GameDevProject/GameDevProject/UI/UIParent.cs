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
using GameDevProject.UI.HUD;
using GameDevProject.UI.Executables;

namespace GameDevProject.UI
{
    public class UIParent
    {
        #region variables
        public bool dead = false;
        public HUDParent HUD;
        public Menu deathMenu;
        public Menu EndOfLevel;
        public Menu MainMenu;
        public Menu End;
        #endregion

        #region constructors
        public UIParent()
        {
            HUD = new HUDParent();
            deathMenu = new Menu
                (
                    new Interfaces.Button[]
                    {
                        new Interfaces.Button
                        (
                            new Rectangle(25, 100, 150, 50),
                            new Restart(),
                            Color.DarkRed,
                            Color.Red,
                            "Restart",
                            2
                        )
                    },
                    "You Died",
                    Color.Red,
                    new Vector2(10, 10), 3
                );
            EndOfLevel = new Menu
                (
                    new Interfaces.Button[]
                    {
                        new Interfaces.Button
                        (
                            new Rectangle(25, 100, 150, 50),
                            new NextLevel(),
                            Color.DarkRed,
                            Color.Red,
                            "Next Level",
                            2
                        )
                    },
                    "You Completed the Level",
                    Color.Red,
                    new Vector2(10, 10),
                    3
                );
            MainMenu = new Menu
                (
                    new Interfaces.Button[]
                    {
                        new Interfaces.Button
                        (
                            new Rectangle(25, 100, 150, 50),
                            new NextLevel(),
                            Color.DarkRed,
                            Color.Red,
                            "Start",
                            2
                        ),
                        new Interfaces.Button
                        (
                            new Rectangle(25, 160, 150, 50),
                            new Exit(),
                            Color.DarkRed,
                            Color.Red,
                            "Exit",
                            2
                        ),
                        new Interfaces.Button
                        (
                            new Rectangle(25, 220, 0, 0),
                            new Empty(),
                            Color.DarkRed,
                            Color.Red,
                            "Movement: arrow keys",
                            1
                        ),
                        new Interfaces.Button
                        (
                            new Rectangle(25, 260, 0, 0),
                            new Empty(),
                            Color.DarkRed,
                            Color.Red,
                            "Shooting: Numpad 0",
                            1
                        )
                    },
                    "Lost Lives",
                    Color.Red,
                    new Vector2(10, 10),
                    3
                );
            End = new Menu
                (
                    new Interfaces.Button[]
                    {
                        new Interfaces.Button
                        (
                            new Rectangle(25, 100, 150, 50),
                            new GotoMainMenu(),
                            Color.DarkRed,
                            Color.Red,
                            "Main Menu",
                            2
                        ),
                        new Interfaces.Button
                        (
                            new Rectangle(25, 160, 150, 50),
                            new Exit(),
                            Color.DarkRed,
                            Color.Red,
                            "Exit",
                            2
                        )
                    },
                    "Thank you for playing!",
                    Color.Red,
                    new Vector2(10, 10),
                    3
                );
        }
        #endregion
        public void Update()
        {
            if(Globals.currWorld.currLevel == 0)
            {
                MainMenu.Update();
            }
            else if(Globals.currWorld.currLevel == Globals.currWorld.levels.Length - 1)
            {
                End.Update();
            }
            else if(Globals.currWorld.levels[Globals.currWorld.currLevel].finished)
            {
                EndOfLevel.Update();
            }
            else if (dead)
            {
                deathMenu.Update();
            }
            else
            {
                HUD.Update();
            }
        }
        public void Draw()
        {
            if (Globals.currWorld.currLevel == 0)
            {
                MainMenu.Draw();
            }
            else if (Globals.currWorld.currLevel == Globals.currWorld.levels.Length - 1)
            {
                End.Draw();
            }
            else if (Globals.currWorld.levels[Globals.currWorld.currLevel].finished)
            {
                EndOfLevel.Draw();
            }
            else if (dead)
            {
                deathMenu.Draw();
            }
            else
            {
                HUD.Draw();
            }
        }
    }
}