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
        Rectangle[,] tiles;
        #region tile positions
        Rectangle[] groundTilePos = new Rectangle[]
        {
            new Rectangle(224, 367, 32, 33),
            new Rectangle(336, 367, 32, 33),
            new Rectangle(384, 367, 32, 33),
            new Rectangle(432, 367, 32, 33)
        };
        Rectangle[] edgeGroundTilePos = new Rectangle[]
        {
            new Rectangle(400, 175, 32, 33),
            new Rectangle(256, 175, 32, 33)
        };
        Rectangle[] borderTilePos = new Rectangle[]
        {
            new Rectangle(432, 320, 32, 32),
            new Rectangle(224, 320, 32, 32)
        };
        Rectangle[] ceilingTilesPos = new Rectangle[]
        {
            new Rectangle(224, 96, 32, 36),
            new Rectangle(304, 96, 32, 32),
            new Rectangle(416, 96, 32, 32)
        };
        Rectangle[] wallAdjacentFloorPos = new Rectangle[]
        {
            new Rectangle(32, 159, 32, 49),
            new Rectangle(80, 159, 32, 49),
            new Rectangle(128, 160, 32, 48),
            new Rectangle(528, 160, 32, 48),
            new Rectangle(576, 159, 32, 49),
            new Rectangle(624, 159, 32, 49)
        };
        Rectangle[][] voidFillerPos = new Rectangle[][]
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
        private Rectangle collisionBox;
        public float bounciness;
        private Vector2 normalDims = new Vector2(32, 32);
        private float[] baseFloorPos = new float[] { float.NaN, float.NaN };
        private Vector2 totalDims;

        public Platform(int _length, int _height, Vector2 _pos, WallAdjacent _adjacencyTop = WallAdjacent.None, string _leftIntersectPos = "", string _rightIntersectPos = "", float _bounciness = 0f, bool _isGround = true) : base("Tiles\\mainlev_build", _pos, new Vector2(32, 33))
        {
            tiles = new Rectangle[_height,_length];

            totalDims = new Vector2(_length, _height);

            int totalLength = _length * 32;

            bounciness = _bounciness;

            switch(_adjacencyTop)
            {
                case WallAdjacent.None:

                    tiles[0, 0] = edgeGroundTilePos[0];
                    tiles[0, _length - 1] = edgeGroundTilePos[1];

                    tiles[_height - 1, 0] = edgeGroundTilePos[0];
                    tiles[_height - 1, _length - 1] = edgeGroundTilePos[1];

                    break;
                case WallAdjacent.Left:

                    tiles[0,0] = PickRectangle(wallAdjacentFloorPos, -0.5f);
                    tiles[0,_length - 1] = edgeGroundTilePos[1];

                    tiles[_height - 1, 0] = PickRectangle(voidFillerPos[Globals.rng.Next(0, 2)]);
                    tiles[_height - 1, _length - 1] = edgeGroundTilePos[1];

                    break;
                case WallAdjacent.Right:

                    tiles[0,0] = edgeGroundTilePos[0];
                    tiles[0,_length - 1] = PickRectangle(wallAdjacentFloorPos, 0.5f);

                    tiles[_height - 1, 0] = edgeGroundTilePos[0];
                    tiles[_height - 1, _length - 1] = PickRectangle(voidFillerPos[Globals.rng.Next(0, 2)]);

                    break;
                case WallAdjacent.Both:

                    tiles[0,0] = PickRectangle(wallAdjacentFloorPos, -0.5f);
                    tiles[0,_length - 1] = PickRectangle(wallAdjacentFloorPos, 0.5f);

                    tiles[_height - 1, 0] = PickRectangle(voidFillerPos[Globals.rng.Next(0, 2)]);
                    tiles[_height - 1, _length - 1] = PickRectangle(voidFillerPos[Globals.rng.Next(0, 2)]);

                    break;
            }

            float[] leftIntersectPos = ToFloatArray(_leftIntersectPos);
            float[] rightIntersectPos = ToFloatArray(_rightIntersectPos);

            for (int x = 1; x < _length - 1; x++)
            {
                tiles[0,x] = PickRectangle(groundTilePos);
            }

            for(int y = 1; y < _height - 1; y++)
            {

                if (isVoid(leftIntersectPos, y))
                {
                    tiles[y, 0] = PickRectangle(voidFillerPos[Globals.rng.Next(0, 2)]);
                }
                else
                {
                    tiles[y, 0] = borderTilePos[0];
                }

                if (leftIntersectPos.Contains(y))
                {
                    tiles[y, 0] = PickRectangle(voidFillerPos[1]);
                }

                for (int x = 1; x < _length - 1; x++)
                {
                    tiles[y, x] = PickRectangle(voidFillerPos[Globals.rng.Next(0, 2)]);
                }

                if (isVoid(rightIntersectPos, y))
                {
                    tiles[y, _length - 1] = PickRectangle(voidFillerPos[Globals.rng.Next(0, 2)]);
                }
                else
                {
                    tiles[y, _length - 1] = borderTilePos[1];
                }

                if (rightIntersectPos.Contains(y))
                {
                    tiles[y, _length - 1] = PickRectangle(voidFillerPos[1]);
                }
            }

            if (_height > 1)
            {
                for (int x = 1; x < _length - 1; x++)
                {
                    tiles[_height - 1, x] = PickRectangle(ceilingTilesPos);
                }
            }
            else if(_length > 1)
            {
                for (int x = 1; x < _length - 1; x++)
                {
                    tiles[0, x] = PickRectangle(groundTilePos);
                }
            }

            collisionBox = new Rectangle((int)_pos.X, (int)_pos.Y, _length * 32, _height * 32);
        }

        private bool isVoid(float[] leftIntersectPos, int y)
        {
            bool isVoid = false;
            for (int i = 0; i < leftIntersectPos.Length; i++)
            {
                if (y > leftIntersectPos[i])
                    isVoid = !isVoid;
                if (y < leftIntersectPos[i])
                {
                    return isVoid;
                }
            }
            return isVoid;
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

        public bool CollisionCheck(CollisionObject obj)
        {
            return GetCollisionBox().Intersects(obj.GetCollisionBox());
        }
        public override void Draw()
        {
            if (totalDims.Y > 1)
            {
                for (int y = 0; y < totalDims.Y - 1; y++)
                {
                    for (int x = 0; x < totalDims.X; x++)
                    {
                        base.Draw(tiles[y, x], new Vector2(tiles[y, x].Width, tiles[y, x].Height) - normalDims, new Vector2(x * 32, y * 32 + normalDims.Y - tiles[y, x].Height));
                    }
                }
                base.Draw(tiles[(int)(totalDims.Y - 1), 0], new Vector2(tiles[(int)(totalDims.Y - 1), 0].Width, tiles[(int)(totalDims.Y - 1), 0].Height) - normalDims, new Vector2(0, (totalDims.Y - 1) * 32), SpriteEffects.FlipVertically);
                for (int x = 1; x < totalDims.X - 1; x++)
                {
                    base.Draw(tiles[(int)(totalDims.Y - 1), x], new Vector2(tiles[(int)(totalDims.Y - 1), x].Width, tiles[(int)(totalDims.Y - 1), x].Height) - normalDims, new Vector2(x * 32, (totalDims.Y - 1) * 32));
                }
                base.Draw(tiles[(int)(totalDims.Y - 1), (int)(totalDims.X - 1)], new Vector2(tiles[(int)(totalDims.Y - 1), (int)(totalDims.X - 1)].Width, tiles[(int)(totalDims.Y - 1), (int)(totalDims.X - 1)].Height) - normalDims, new Vector2((totalDims.X - 1) * 32, (totalDims.Y - 1) * 32), SpriteEffects.FlipVertically);
            }
            else
            {
                if (totalDims.X > 1)
                {
                    for (int y = 0; y < totalDims.Y; y++)
                    {
                        for (int x = 0; x < totalDims.X; x++)
                        {
                            base.Draw(tiles[y, x], new Vector2(tiles[y, x].Width, tiles[y, x].Height) - normalDims, new Vector2(x * 32, y * 32 + normalDims.Y - tiles[y, x].Height));
                        }
                    }
                }
                else
                {
                    for (int y = 0; y < totalDims.Y - 1; y++)
                    {
                        for (int x = 0; x < totalDims.X; x++)
                        {
                            base.Draw(tiles[y, x], new Vector2(tiles[y, x].Width, tiles[y, x].Height) - normalDims, new Vector2(x * 32, y * 32 + normalDims.Y - tiles[y, x].Height));
                        }
                    }
                }
            }
        }
        public float GetBounciness()
        {
            return bounciness;
        }
        public Rectangle GetCollisionBox()
        {
            return collisionBox;
        }
        private Rectangle PickRectangle(Rectangle[] choices, float portion = 0)
        {
            switch(portion)
            {
                case -0.5f:
                    return choices[Globals.rng.Next(0, choices.Length / 2)];
                default:
                    return choices[Globals.rng.Next(0, choices.Length)];
                case 0.5f:
                    return choices[Globals.rng.Next(choices.Length / 2, choices.Length)];
            }
        }
    }
}