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
using GameDevProject.Engine.Gamestructure.WorldObjects;
using GameDevProject.Engine.Gamestructure.WorldObjects.Entities;

namespace GameDevProject.Engine.Gamestructure
{
    public class Level : Sprite
    {
        #region variables
        public Platform[] platforms;
        public Entity[] entities;
        public Texture2D background;
        public Vector2 spawn;
        public float endOfLevelX;
        public bool finished = false;
        #endregion

        #region Constructor
        public Level(Rectangle[] _platforms, Entity[] _entities, Vector2 _spawn, float _endOfLevelX, string _path = "") : base(_path, Vector2.Zero, Globals.screenSize)
        {
            platforms = RectArrToPlatformArr(_platforms);
            entities = _entities;
            spawn = _spawn;
            if (_path != "")
            {
                background = Globals.content.Load<Texture2D>(_path);
            }
            endOfLevelX = _endOfLevelX;
        }
        #region Support methods
        private Platform[] RectArrToPlatformArr(Rectangle[] input)
        {
            Platform[] output = new Platform[input.Length];
            for (int i = 0; i < input.Length; i++)
            {
                string[] intersects = GetIntersects(input[i], input, i);
                output[i] = new Platform(input[i].Width, input[i].Height, new Vector2(input[i].X, input[i].Y), intersects[0], intersects[1], intersects[2], intersects[3]);
            }
            return output;
        }
        private string[] GetIntersects(Rectangle rect, Rectangle[] collRect, int iToExclude)  //left, top, right, bottom
        {
            #region variable declaration
            string[] output = new string[4];
            for (int i = 0; i < 4; i++)
            {
                output[i] = "";
            }

            List<int>[] intersects = new List<int>[4];
            for (int i = 0; i < 4; i++)
            {
                intersects[i] = new List<int>();
            }
            #endregion

            for (int i = 0; i < collRect.Length; i++)
            {
                if (i != iToExclude)
                {
                    #region left intersects
                    if (collRect[i].Right >= rect.Left && collRect[i].Left <= rect.Left)
                    {
                        if (collRect[i].Top <= rect.Bottom && collRect[i].Bottom >= rect.Top)
                        {
                            if (collRect[i].Top < rect.Top)
                            {
                                if (collRect[i].Bottom > rect.Bottom)
                                {
                                    intersects[0].Add(rect.Top - rect.Y);
                                    intersects[0].Add(rect.Bottom - rect.Y);
                                }
                                else if (collRect[i].Bottom < rect.Bottom)
                                {
                                    intersects[0].Add(rect.Top - rect.Y);
                                    intersects[0].Add(collRect[i].Bottom - rect.Y);
                                }
                            }
                            else if (collRect[i].Top > rect.Top)
                            {
                                if (collRect[i].Bottom > rect.Bottom)
                                {
                                    intersects[0].Add(collRect[i].Top - rect.Y);
                                    intersects[0].Add(rect.Bottom - rect.Y);
                                }
                                else if (collRect[i].Bottom < rect.Bottom)
                                {
                                    intersects[0].Add(collRect[i].Top - rect.Y);
                                    intersects[0].Add(collRect[i].Bottom - rect.Y);
                                }
                            }
                        }
                    }
                    #endregion
                    #region top intersects
                    if (collRect[i].Top <= rect.Top && collRect[i].Bottom >= rect.Top)
                    {
                        if (collRect[i].Left <= rect.Right && collRect[i].Right >= rect.Left)
                        {
                            if (collRect[i].Left < rect.Left)
                            {
                                if (collRect[i].Right > rect.Right)
                                {
                                    intersects[1].Add(rect.Left - rect.X);
                                    intersects[1].Add(rect.Right - rect.X);
                                }
                                else if (collRect[i].Right < rect.Right)
                                {
                                    intersects[1].Add(rect.Left - rect.X);
                                    intersects[1].Add(collRect[i].Right - rect.X);
                                }
                            }
                            else if (collRect[i].Left > rect.Left)
                            {
                                if (collRect[i].Right > rect.Right)
                                {
                                    intersects[1].Add(collRect[i].Left - rect.X);
                                    intersects[1].Add(rect.Right - rect.X);
                                }
                                else if (collRect[i].Right < rect.Right)
                                {
                                    intersects[1].Add(collRect[i].Left - rect.X);
                                    intersects[1].Add(collRect[i].Right - rect.X);
                                }
                            }
                        }
                    }
                    #endregion
                    #region right intersects
                    if (collRect[i].Left <= rect.Right && collRect[i].Right >= rect.Right)
                    {
                        if (collRect[i].Top <= rect.Bottom && collRect[i].Bottom >= rect.Top)
                        {
                            if (collRect[i].Top < rect.Top)
                            {
                                if (collRect[i].Bottom > rect.Bottom)
                                {
                                    intersects[2].Add(rect.Top - rect.Y);
                                    intersects[2].Add(rect.Bottom - rect.Y);
                                }
                                else if (collRect[i].Bottom < rect.Bottom)
                                {
                                    intersects[2].Add(rect.Top - rect.Y);
                                    intersects[2].Add(collRect[i].Bottom - rect.Y);
                                }
                            }
                            else if (collRect[i].Top > rect.Top)
                            {
                                if (collRect[i].Bottom > rect.Bottom)
                                {
                                    intersects[2].Add(collRect[i].Top - rect.Y);
                                    intersects[2].Add(rect.Bottom - rect.Y);
                                }
                                else if (collRect[i].Bottom < rect.Bottom)
                                {
                                    intersects[2].Add(collRect[i].Top - rect.Y);
                                    intersects[2].Add(collRect[i].Bottom - rect.Y);
                                }
                            }
                        }
                    }
                    #endregion
                    #region bot intersects
                    if (collRect[i].Top <= rect.Bottom && collRect[i].Bottom >= rect.Bottom)
                    {
                        if (collRect[i].Left <= rect.Right && collRect[i].Right >= rect.Left)
                        {
                            if (collRect[i].Left < rect.Left)
                            {
                                if (collRect[i].Right > rect.Right)
                                {
                                    intersects[3].Add(rect.Left - rect.X);
                                    intersects[3].Add(rect.Right - rect.X);
                                }
                                else if (collRect[i].Right < rect.Right)
                                {
                                    intersects[3].Add(rect.Left - rect.X);
                                    intersects[3].Add(collRect[i].Right - rect.X);
                                }
                            }
                            else if (collRect[i].Left > rect.Left)
                            {
                                if (collRect[i].Right > rect.Right)
                                {
                                    intersects[3].Add(collRect[i].Left - rect.X);
                                    intersects[3].Add(rect.Right - rect.X);
                                }
                                else if (collRect[i].Right < rect.Right)
                                {
                                    intersects[3].Add(collRect[i].Left - rect.X);
                                    intersects[3].Add(collRect[i].Right - rect.X);
                                }
                            }
                        }
                    }
                    #endregion
                }
            }

            #region convert to string
            for (int i = 0; i < intersects.Length; i++)
            {
                if (intersects[i].Count > 0)
                {
                    intersects[i].Sort();
                    output[i] += intersects[i][0];
                    for (int j = 1; j < intersects[i].Count; j++)
                    {
                        output[i] += $",{intersects[i][j]}";
                    }
                }
            }
            #endregion

            return output;
        }
        #endregion
        #endregion
        #region Variable return
        public CollisionObject[] CollisionObjects()
        {
            CollisionObject[] objects = new CollisionObject[platforms.Length + entities.Length];
            for(int i = 0; i < platforms.Length; i++)
            {
                objects[i] = platforms[i];
            }
            for(int i = 0; i < entities.Length; i++)
            {
                objects[platforms.Length + i] = entities[i];
            }
            return objects;
        }
        #endregion
        #region Sprite
        public override void Draw()
        {
            foreach (Entity entity in entities)
            {
                entity.Draw();
            }
            foreach (Platform platform in platforms)
            {
                platform.Draw();
            }
        }
        public override void Update()
        {
            if (!finished)
            {
                foreach (Platform platform in platforms)
                {
                    platform.Update();
                }
                foreach (Entity entity in entities)
                {
                    entity.Update();
                }
            }
        }
        #endregion
        #region Level
        public void StartLevel()
        {
            for(int i = 0; i < entities.Length; i++)
            {
                if (entities[i].GetType() == typeof(Hero))
                    ((Sprite)entities[i]).pos = spawn;
            }
        }
        public void EntityReset()
        {
            for (int i = 0; i < entities.Length; i++)
            {
                Entity temp = entities[i].Clone();
                if (entities[i].GetType() == typeof(Hero))
                    Globals.currWorld.hero = (Hero)temp;
                entities[i] = temp;
            }
            finished = false;
        }
        #endregion
    }
}