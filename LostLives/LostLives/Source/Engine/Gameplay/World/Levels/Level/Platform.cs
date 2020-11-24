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
    public enum TrailOffs { None, Left, Right, Both}
    public class Platform : Sprite, CollisionObject
    {
        Rectangle[] tiles;
        TrailOffs trailOff;
        Rectangle[] groundTilePos = new Rectangle[] { new Rectangle(224, 367, 32, 33), new Rectangle(336, 367, 32, 33), new Rectangle(384, 367, 32, 33), new Rectangle(432, 367, 32, 33) };
        private Rectangle collisionBox;
        public float bounciness;

        public Platform(int _length, Vector2 _pos, float _bounciness = 0f, TrailOffs _trailOff = TrailOffs.None, bool _isGround = true) : base("Tiles\\mainlev_build", _pos, new Vector2(32, 33))
        {
            tiles = new Rectangle[_length];

            int totalLength = 0;

            bounciness = _bounciness;

            trailOff = _trailOff;

            int startIndex = 0;
            int endIndex = tiles.Length;

            switch(trailOff)
            {
                case TrailOffs.Left:

                    tiles[0] = new Rectangle(400, 175, 32, 33);
                    totalLength += 32;
                    startIndex++;

                    break;
                case TrailOffs.Right:

                    tiles[tiles.Length - 1] = new Rectangle(256, 175, 32, 33);
                    totalLength += 32;
                    endIndex--;

                    break;
                case TrailOffs.Both:

                    tiles[0] = new Rectangle(400, 175, 32, 33);
                    totalLength += 32;
                    startIndex++;

                    tiles[tiles.Length - 1] = new Rectangle(256, 175, 32, 33);
                    totalLength += 32;
                    endIndex--;

                    break;
            }

            for (int i = startIndex; i < endIndex; i++)
            {
                tiles[i] = groundTilePos[Globals.rng.Next(0, 4)];
                totalLength += 32;
            }

            collisionBox = new Rectangle((int)_pos.X, (int)_pos.Y, totalLength, 33);
        }

        public bool CollisionCheck(CollisionObject obj)
        {
            return GetCollisionBox().Intersects(obj.GetCollisionBox());
        }

        public override void Draw()
        {
            int frontOffset = 0;

            int startIndex = 0;
            int endIndex = tiles.Length;

            switch(trailOff)
            {
                case TrailOffs.Left:

                    startIndex++;
                    frontOffset = 32;
                    base.Draw(tiles[0], Vector2.Zero);

                    break;
                case TrailOffs.Right:

                    endIndex--;
                    base.Draw(tiles[endIndex], Vector2.Zero, new Vector2((tiles.Length - 1) * 32, 0));

                    break;
                case TrailOffs.Both:

                    startIndex++;
                    frontOffset = 32;
                    base.Draw(tiles[0], Vector2.Zero);

                    endIndex--;
                    base.Draw(tiles[endIndex], Vector2.Zero, new Vector2((tiles.Length - 1) * 32, 0));

                    break;
            }

            for(int i = startIndex; i < endIndex; i++)
            {
                base.Draw(tiles[i], Vector2.Zero, new Vector2(frontOffset + (i - startIndex) * 32, 0));
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
    }
}