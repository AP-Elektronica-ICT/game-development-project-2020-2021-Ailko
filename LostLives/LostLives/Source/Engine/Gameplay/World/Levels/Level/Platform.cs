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
    public enum WallAdjacent { None, Left, Right, Both }
    public class Platform : Sprite, CollisionObject
    {
        #region variables
        private Rectangle[,] tiles;
        private Rectangle collisionBox;
        private Vector2 totalDims;
        public float bounciness;
        private bool[,] cornerBorders;
        #endregion
        #region constants
        private Vector2 normalDims = new Vector2(32, 32);
        #region tile positions
        Rectangle[] floorTiles = new Rectangle[]
        {
            new Rectangle(224, 367, 32, 33),
            new Rectangle(336, 367, 32, 33),
            new Rectangle(384, 367, 32, 33),
            new Rectangle(432, 367, 32, 33)
        };
        Rectangle[] edgeFloorTiles = new Rectangle[]
        {
            new Rectangle(400, 175, 32, 33),
            new Rectangle(256, 175, 32, 33)
        };
        Rectangle[] borderTiles = new Rectangle[]
        {
            new Rectangle(432, 320, 32, 32),
            new Rectangle(224, 320, 32, 32)
        };
        Rectangle[] ceilingTiles = new Rectangle[]
        {
            new Rectangle(224, 96, 32, 36),
            new Rectangle(304, 96, 32, 32),
            new Rectangle(416, 96, 32, 32)
        };
        Rectangle[] wallAdjacentCeilingTiles = new Rectangle[]
        {
            new Rectangle(32, 80, 32, 48),
            new Rectangle(624, 80, 32, 48)
        };
        Rectangle[] wallAdjacentFloorTiles = new Rectangle[]
        {
            new Rectangle(32, 159, 32, 49),
            new Rectangle(80, 159, 32, 49),
            new Rectangle(128, 160, 32, 48),
            new Rectangle(528, 160, 32, 48),
            new Rectangle(576, 159, 32, 49),
            new Rectangle(624, 159, 32, 49)
        };
        Rectangle[][] voidFillerTiles = new Rectangle[][]
        {
            new Rectangle[]
            {
                new Rectangle(32, 32, 32, 32),
                new Rectangle(272, 32, 32, 32),
                new Rectangle(320, 16, 32, 32),
                new Rectangle(128, 448, 32, 32),
                new Rectangle(240, 448, 32, 32),
                new Rectangle(416, 448, 32, 32),
                new Rectangle(528, 448, 32, 32),
                new Rectangle(240, 496, 32, 32),
                new Rectangle(416, 496, 32, 32)
            },
            new Rectangle[]
            {
                new Rectangle(32, 32, 32, 32),
                new Rectangle(272, 32, 32, 32),
                new Rectangle(320, 16, 32, 32),
                new Rectangle(240, 496, 32, 32),
                new Rectangle(416, 496, 32, 32)
            }
        };
        #endregion
        #endregion

        public Platform(int _width, int _height, Vector2 _pos, string _leftIntersectPos = "", string _topIntersectPos = "", string _rightIntersectPos = "", string _botIntersectPos = "", float _bounciness = 0f, bool _isGround = true) : base("Tiles\\mainlev_build", _pos * 32, new Vector2(32, 32))
        {
            bounciness = _bounciness;
            totalDims = new Vector2(_width, _height);
            collisionBox = new Rectangle((int)_pos.X * 32, (int)_pos.Y * 32, _width * 32, _height * 32);

            float[] leftIntersects = ToFloatArray(_leftIntersectPos);
            if(leftIntersects.Length > 0 && leftIntersects[0] == pos.Y)
            {
                float[] temp = new float[leftIntersects.Length + 1];
                temp[0] = leftIntersects[0];
                for(int i = 0; i < leftIntersects.Length; i++)
                {
                    temp[i + 1] = leftIntersects[i];
                }
                leftIntersects = temp;
            }

            float[] rightIntersects = ToFloatArray(_rightIntersectPos);
            if (rightIntersects.Length > 0 && rightIntersects[0] == pos.Y)
            {
                float[] temp = new float[rightIntersects.Length + 1];
                temp[0] = rightIntersects[0];
                for (int i = 0; i < rightIntersects.Length; i++)
                {
                    temp[i + 1] = rightIntersects[i];
                }
                rightIntersects = temp;
            }

            float[] topIntersects = ToFloatArray(_topIntersectPos);
            if (topIntersects.Length > 0 && topIntersects[0] == pos.X)
            {
                float[] temp = new float[topIntersects.Length + 1];
                temp[0] = topIntersects[0];
                for (int i = 0; i < topIntersects.Length; i++)
                {
                    temp[i + 1] = topIntersects[i];
                }
                topIntersects = temp;
            }

            float[] botIntersects = ToFloatArray(_botIntersectPos);
            if (botIntersects.Length > 0 && botIntersects[0] == pos.X)
            {
                float[] temp = new float[botIntersects.Length + 1];
                temp[0] = botIntersects[0];
                for (int i = 0; i < botIntersects.Length; i++)
                {
                    temp[i + 1] = botIntersects[i];
                }
                botIntersects = temp;
            }

            tiles = new Rectangle[_height, _width];
            cornerBorders = new bool[2, 2];

            if(_height > 1)
            {
                if(_width > 1)
                {
                    #region top row

                    #region left tile
                    if (isVoid(topIntersects, 0))
                    {
                        tiles[0, 0] = PickTile(wallAdjacentFloorTiles, -0.5f);
                        cornerBorders[0, 0] = true;
                    }
                    else
                        tiles[0, 0] = edgeFloorTiles[0];
                    #endregion
                    #region middle tiles
                    for (int x = 1; x < _width - 1; x++)
                    {
                        if(isVoid(topIntersects, x))
                        {
                            if(topIntersects.Contains(x))
                            {
                                tiles[0, x] = PickTile(voidFillerTiles[1]);
                            }
                            else
                            {
                                tiles[0, x] = PickTile(voidFillerTiles[Globals.rng.Next(0, 2)]);
                            }
                        }
                        else
                        {
                            tiles[0, x] = PickTile(floorTiles);
                        }
                    }
                    #endregion
                    #region right tile
                    if (isVoid(topIntersects, _width))
                    {
                        tiles[0, _width - 1] = wallAdjacentFloorTiles[1];
                        cornerBorders[0, 1] = true;
                    }
                    else
                        tiles[0, _width - 1] = edgeFloorTiles[1];
                    #endregion

                    #endregion
                    #region middle rows
                    for (int y = 1; y < _height - 1; y++)
                    {
                        #region left tile
                        if(isVoid(leftIntersects, y))
                        {
                            if (leftIntersects.Contains(y))
                                tiles[y, 0] = PickTile(voidFillerTiles[1]);
                            else
                                tiles[y, 0] = PickTile(voidFillerTiles[Globals.rng.Next(0, 2)]);
                        }
                        else
                        {
                            tiles[y, 0] = borderTiles[0];
                        }
                        #endregion

                        #region middle tiles
                        for (int x = 1; x < _width - 1; x++)
                        {
                            tiles[y, x] = PickTile(voidFillerTiles[Globals.rng.Next(0, 2)]);
                        }
                        #endregion

                        #region right tile
                        if (isVoid(rightIntersects, y))
                        {
                            if (rightIntersects.Contains(y) || rightIntersects.Contains(y + 1))
                                tiles[y, _width - 1] = PickTile(voidFillerTiles[1]);
                            else
                                tiles[y, _width - 1] = PickTile(voidFillerTiles[Globals.rng.Next(0, 2)]);
                        }
                        else
                        {
                            tiles[y, _width - 1] = borderTiles[1];
                        }
                        #endregion
                    }
                    #endregion
                    #region bot row

                    #region left tile
                    if (isVoid(botIntersects, 0))
                    {
                        if (botIntersects[0] == 0 && botIntersects[0] != botIntersects[1])
                        {
                            tiles[_height - 1, 0] = borderTiles[0];
                        }
                        else
                        {
                            tiles[_height - 1, 0] = wallAdjacentCeilingTiles[0];
                        }
                        cornerBorders[1, 0] = true;
                    }
                    else
                        tiles[_height - 1, 0] = edgeFloorTiles[0];
                    #endregion
                    #region middle tiles
                    for (int x = 1; x < _width - 1; x++)
                    {
                        if (isVoid(botIntersects, x))
                        {
                            if (botIntersects.Contains(x))
                                tiles[_height - 1, x] = PickTile(voidFillerTiles[1]);
                            else
                                tiles[_height - 1, x] = PickTile(voidFillerTiles[Globals.rng.Next(0, 2)]);
                        }
                        else
                        {
                            tiles[_height - 1, x] = PickTile(ceilingTiles);
                        }
                    }
                    #endregion
                    #region right tile
                    if (isVoid(botIntersects, _width - 1))
                    {
                        if (botIntersects[(int)totalDims.Y - 1] == totalDims.X)
                        {
                            tiles[_height - 1, _width - 1] = borderTiles[1];
                        }
                        else
                        {
                            tiles[_height - 1, _width - 1] = wallAdjacentCeilingTiles[1];
                        }
                        cornerBorders[1, 1] = true;
                    }
                    else
                        tiles[_height - 1, _width - 1] = edgeFloorTiles[1];
                    #endregion

                    #endregion
                }
                else
                {
                    #region top tile
                    if (topIntersects.Contains(0))
                    {
                        tiles[0, 0] = wallAdjacentFloorTiles[0];
                        cornerBorders[0, 0] = true;
                        cornerBorders[0, 1] = true;
                    }
                    else if (topIntersects.Contains(1))
                    {
                        tiles[0, 0] = wallAdjacentFloorTiles[1];
                        cornerBorders[0, 0] = true;
                        cornerBorders[0, 1] = true;
                    }
                    else
                        tiles[0, 0] = PickTile(floorTiles);
                    #endregion
                    #region middle tiles
                    for(int y = 1; y < _height - 1; y++)
                    {
                        tiles[y, 0] = PickTile(voidFillerTiles[Globals.rng.Next(0, 2)]);
                    }
                    #endregion
                    #region bot tile
                    if (botIntersects.Contains(0))
                    {
                        tiles[_height - 1, 0] = wallAdjacentCeilingTiles[0];
                        cornerBorders[1, 0] = true;
                        cornerBorders[1, 1] = true;
                    }
                    else if (botIntersects.Contains(1))
                    {
                        tiles[_height - 1, 0] = wallAdjacentCeilingTiles[1];
                        cornerBorders[0, 0] = true;
                        cornerBorders[0, 1] = true;
                    }
                    else
                        tiles[_height - 1, 0] = PickTile(ceilingTiles);
                    #endregion
                }
            }
            else
            {
                if(_width > 1)
                {
                    #region left tile
                    if(topIntersects.Contains(0))
                    {
                        tiles[0, 0] = PickTile(wallAdjacentFloorTiles, -0.5f);
                        cornerBorders[0, 0] = true;
                        cornerBorders[1, 0] = true;
                    }
                    else
                        tiles[0, 0] = edgeFloorTiles[0];
                    #endregion
                    #region middle tiles
                    for(int x = 1; x < _width - 1; x++)
                    {
                        if(isVoid(topIntersects, x))
                        {
                            if(topIntersects.Contains(x))
                            {
                                tiles[0, x] = PickTile(voidFillerTiles[1]);
                            }
                            else
                            {
                                tiles[0, x] = PickTile(voidFillerTiles[Globals.rng.Next(0, 2)]);
                            }
                        }
                        else
                        {
                            tiles[0, x] = PickTile(floorTiles);
                        }
                    }
                    #endregion
                    #region right tile
                    if (topIntersects.Contains(_width))
                    {
                        tiles[0, _width - 1] = PickTile(wallAdjacentFloorTiles, 0.5f);
                        cornerBorders[1, 1] = true;
                        cornerBorders[1, 1] = true;
                    }
                    else
                    {
                        tiles[0, _width - 1] = edgeFloorTiles[1];
                    }
                    #endregion
                }
                else
                {
                    tiles[0, 0] = PickTile(floorTiles);
                }
            }
        }

        #region constructor methods
        private Rectangle PickTile(Rectangle[] choices, float portion = 0)
        {
            switch (portion)
            {
                case -0.5f:
                    return choices[Globals.rng.Next(0, choices.Length / 2)];
                default:
                    return choices[Globals.rng.Next(0, choices.Length)];
                case 0.5f:
                    return choices[Globals.rng.Next(choices.Length / 2, choices.Length)];
            }
        }
        private float[] ToFloatArray(string input)
        {
            string[] temp = input.Split(',');
            float[] output = new float[temp.Length];

            if (input != "")
            {
                for (int i = 0; i < temp.Length; i++)
                {
                    output[i] = (float)Convert.ToDouble(temp[i]);
                }
            }
            else
            {
                output = new float[] { };
            }
            return output;
        }
        private bool isVoid(float[] IntersectPoints, int compareTo)
        {
            bool isVoid = false;
            for (int i = 0; i < IntersectPoints.Length; i++)
            {
                if (compareTo >= IntersectPoints[i])
                    isVoid = !isVoid;
                if (compareTo <= IntersectPoints[i])
                    return isVoid;
            }
            return isVoid;
        }
        #endregion
        #region collision methods
        public bool CollisionCheck(CollisionObject obj)
        {
            return GetCollisionBox().Intersects(obj.GetCollisionBox());
        }
        public float GetBounciness()
        {
            return bounciness;
        }
        public Rectangle GetCollisionBox()
        {
            return collisionBox;
        }
        #endregion
        public override void Draw()
        {
            if (totalDims.Y > 1)
            {
                if (totalDims.X > 1)
                {
                    #region first row
                    for (int x = 0; x < totalDims.X; x++)
                        base.Draw(tiles[0, x], new Vector2(tiles[0, x].Width, tiles[0, x].Height) - normalDims, new Vector2(x * 32, normalDims.Y - tiles[0, x].Height));
                    #endregion
                    #region middle rows
                    for (int y = 1; y < totalDims.Y - 1; y++)
                    {
                        for(int x = 0; x < totalDims.X; x++)
                        {
                            base.Draw(tiles[y, x], Vector2.Zero, new Vector2(x * 32, y * 32));
                        }
                    }
                    #endregion
                    #region last row
                    #region first tile
                    if (cornerBorders[1, 0])
                        base.Draw(tiles[(int)totalDims.Y - 1, 0], new Vector2(tiles[(int)totalDims.Y - 1, 0].Width, tiles[(int)totalDims.Y - 1, 0].Height) - normalDims, new Vector2(0, (totalDims.Y - 1) * 32));
                    else
                        base.Draw(tiles[(int)totalDims.Y - 1, 0], Vector2.Zero, new Vector2(0, (totalDims.Y - 1) * 32), SpriteEffects.FlipVertically);
                    #endregion
                    #region middle tiles
                    for(int x = 1; x < totalDims.X - 1; x++)
                    {
                        base.Draw(tiles[(int)totalDims.Y - 1, x], Vector2.Zero, new Vector2(x * 32, (totalDims.Y - 1) * 32));
                    }
                    #endregion
                    #region last tile
                    if (!cornerBorders[1, 1])
                        base.Draw(tiles[(int)totalDims.Y - 1, (int)totalDims.X - 1], Vector2.Zero, new Vector2((totalDims.X - 1) * 32, (totalDims.Y - 1) * 32), SpriteEffects.FlipVertically);
                    else
                        base.Draw(tiles[(int)totalDims.Y - 1, (int)totalDims.X - 1], new Vector2(tiles[(int)totalDims.Y - 1, (int)totalDims.X - 1].Width, tiles[(int)totalDims.Y - 1, (int)totalDims.X - 1].Height) - normalDims, new Vector2((totalDims.X - 1) * 32, (totalDims.Y - 1) * 32));
                    #endregion
                    #endregion
                }
                else
                {
                    #region first row
                    if (!cornerBorders[0, 0])
                        base.Draw(tiles[0, 0], Vector2.Zero);
                    else
                        base.Draw(tiles[0, 0], new Vector2(tiles[0, 0].Width, tiles[0, 0].Height) - normalDims, new Vector2(0, normalDims.Y - tiles[0, 0].Height));
                    #endregion
                    #region middle rows
                    for(int y = 1; y < totalDims.Y - 1; y++)
                        base.Draw(tiles[y, 0], Vector2.Zero, new Vector2(0, y * 32));
                    #endregion 
                    #region last row
                    if (!cornerBorders[1, 1])
                        base.Draw(tiles[(int)totalDims.Y - 1, 0], Vector2.Zero, new Vector2(0, (totalDims.Y - 1) * 32));
                    else
                        base.Draw(tiles[(int)totalDims.Y - 1, 0], new Vector2(tiles[(int)totalDims.Y - 1, 0].Width, tiles[(int)totalDims.Y - 1, 0].Height) - normalDims, new Vector2(0, (totalDims.Y - 1) * 32));
                    #endregion
                }
            }
            else
            {
                if (totalDims.X > 1)
                {
                    #region first tile
                    if (!cornerBorders[0, 0])
                        base.Draw(tiles[0, 0], Vector2.Zero);
                    else
                        base.Draw(tiles[0, 0], new Vector2(tiles[0, 0].Width, tiles[0, 0].Height) - normalDims, new Vector2(0, tiles[0, 0].Height - normalDims.Y));
                    #endregion
                    #region middle tiles
                    for(int x = 1; x < totalDims.X - 1; x++)
                    {
                        base.Draw(tiles[0, x], Vector2.Zero, new Vector2(x * 32, 0));
                    }
                    #endregion
                    #region last tile
                    if (!cornerBorders[0, 1])
                        base.Draw(tiles[0, (int)totalDims.X - 1], Vector2.Zero, new Vector2((totalDims.X - 1) * 32, 0));
                    else
                        base.Draw(tiles[0, (int)totalDims.X - 1], new Vector2(tiles[0, (int)totalDims.X - 1].Width, tiles[0, (int)totalDims.X - 1].Height) - normalDims, new Vector2((totalDims.X - 1) * 32, 0));
                    #endregion
                }
                else
                {
                    base.Draw(tiles[0, 0], Vector2.Zero);
                }
            }
        }
    }
}