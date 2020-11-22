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
    public class Hero : Sprite, CollisionObject
    {   // walking width = 41 height = 54, shooting width = 52 height = 53, death width = 57
        private int animationFrame;
        private int frame;

        private float speed = 2;
        private float jumpForce = 7.5f;

        private Vector2 vector = new Vector2(0f, 0f);

        private bool hasJumped = false;
        private float jumpedFromY = 0;

        private bool lastDirRight = false;

        public Hero(string _path, Vector2 _pos, Vector2 _dims) : base(_path, _pos, _dims)
        {
            animationFrame = 0;
            frame = 0;
        }

        public override void Update()
        {
            if(Globals.keyboard.GetPress("Q") || Globals.keyboard.GetPress("Left"))
            {
                if (vector.X > -speed)
                    vector.X -= 0.1f;
            }
            else if (Globals.keyboard.GetPress("D") || Globals.keyboard.GetPress("Right"))
            {
                if (vector.X < speed)
                    vector.X += 0.1f;
            }
            vector.X -= vector.X / 20;

            if ((Globals.keyboard.GetPress("Z") || Globals.keyboard.GetPress("Space") || Globals.keyboard.GetPress("Up")) && !hasJumped)
            {
                hasJumped = true;
                jumpedFromY = pos.Y;
                vector.Y -= jumpForce;
            }
            if(!hasJumped)
            {
                frame = (int)((frame + 1) % (24 / MathF.Sqrt((float)(Math.Pow(vector.X, 2) + Math.Pow(vector.Y, 2)))));
                if(frame == 0)
                    animationFrame = (animationFrame + 1) % 4;
            }

            vector.Y += Globals.gravity * (float)Globals.deltaTime.TotalSeconds - vector.Y * 0.2f;

            int currCollPlatIndex = Globals.currWorld.levels.GetCurrLevel().Update(this);
            if(currCollPlatIndex != -1)
            {
                Platform currCollPlat = Globals.currWorld.levels.GetCurrLevel().platforms[currCollPlatIndex];
                if (currCollPlat.pos.Y > pos.Y)
                {
                    vector.Y -= (float)(9.81f * Globals.deltaTime.TotalSeconds * Math.Pow(this.GetSpeed() / (9.81f * Globals.deltaTime.TotalSeconds), currCollPlat.bounciness));
                }
                else
                {
                    vector.Y += (float)(9.81f * Globals.deltaTime.TotalSeconds * Math.Pow(this.GetSpeed() / (9.81f * Globals.deltaTime.TotalSeconds), currCollPlat.bounciness));
                }
            }

            pos = new Vector2(pos.X + vector.X, pos.Y + vector.Y);
        }

        public bool CollisionCheck(CollisionObject obj)
        {
            return this.GetCollisionBox().Intersects(obj.GetCollisionBox());
        }

        public Rectangle GetCollisionBox()
        {
            return new Rectangle((int)pos.X, (int)pos.Y, (int)dims.X, (int)dims.Y);
        }

        public override void Draw()
        {
            if (Math.Round(vector.X, 1) > 0)
            {
                lastDirRight = true;
                base.Draw(new Rectangle(animationFrame * 41, 0, 41, 54), Vector2.Zero, SpriteEffects.FlipHorizontally);
            }
            else if (Math.Round(vector.X, 1) < 0)
            {
                lastDirRight = false;
                base.Draw(new Rectangle(animationFrame * 41, 0, 41, 54), Vector2.Zero);
            }
            else
            {
                if (lastDirRight)
                    base.Draw(new Rectangle(0, 54, 52, 53), new Vector2(11, -1), SpriteEffects.FlipHorizontally);
                else
                    base.Draw(new Rectangle(0, 54, 52, 53), new Vector2(11, -1));
            }
        }

        private float GetSpeed()
        {
            return (float)Math.Sqrt(Math.Pow(vector.X, 2) + Math.Pow(vector.Y, 2));
        }
    }
}
