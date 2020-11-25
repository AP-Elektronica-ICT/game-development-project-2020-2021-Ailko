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
        #region animation
        private int animationFrame;
        private int frame;
        private bool lastDirRight = false;
        #endregion
        #region physics constants
        private float speed = 2;
        private float walkingAccelaration = 0.1f;
        private float walkingAccelarationMultiplier = 1f;
        private float jumpForce = 3f;
        private float mass = 163.29f;
        #endregion
        #region movement
        private Vector2 vector = new Vector2(0f, 0f);
        private bool hasJumped = false;
        #endregion

        public Hero(string _path, Vector2 _pos, Vector2 _dims) : base(_path, _pos, _dims)
        {
            animationFrame = 0;
            frame = 0;
        }

        public override void Update()
        {
            #region walking
            if (Globals.keyboard.GetPress("Q") || Globals.keyboard.GetPress("Left"))
            {
                if (vector.X > -speed)
                    vector.X -= walkingAccelaration * walkingAccelarationMultiplier;
            }
            else if (Globals.keyboard.GetPress("D") || Globals.keyboard.GetPress("Right"))
            {
                if (vector.X < speed)
                    vector.X += walkingAccelaration * walkingAccelarationMultiplier;
            }
            else if(!hasJumped)
            {
                vector.X -= vector.X * walkingAccelaration;
            }
            #endregion
            #region physics
            Vector2 drag = DragVector();
            drag /= mass;
            Vector2 gravitationalAcceleration = new Vector2(0, Globals.gravity * mass);

            vector += (drag + gravitationalAcceleration) * (float)Globals.deltaTime.TotalSeconds * Globals.metersPerPixel;
            #endregion
            #region collision
            Vector2 collisionVector = Globals.currWorld.levels.GetCurrLevel().CollisionSpecifics(this);
            vector += collisionVector;
            #endregion
            #region jumping
            #region check in air
            if (collisionVector.Y < 0)
            {
                hasJumped = false;
                walkingAccelarationMultiplier = 1f;
            }

            if (vector.Y + collisionVector.Y > 0)
            {
                hasJumped = true;
                walkingAccelarationMultiplier = 0.5f;
            }
            #endregion
            if ((Globals.keyboard.GetPress("Z") || Globals.keyboard.GetPress("Space") || Globals.keyboard.GetPress("Up")) && !hasJumped)
            {
                hasJumped = true;
                vector.Y -= jumpForce;
                walkingAccelarationMultiplier = 0.5f;
            }
            #endregion
            #region vector rounding
            vector.X = (float)Math.Round(vector.X, 3);
            vector.Y = (float)Math.Round(vector.Y, 3);
            #endregion
            #region animation update
            if (!hasJumped)
            {
                frame = (int)((frame + 1) % (24 / MathF.Sqrt((float)(Math.Pow(vector.X, 2) + Math.Pow(vector.Y, 2)))));
                if (frame == 0)
                    animationFrame = (animationFrame + 1) % 4;
            }
            #endregion
            #region position update
            pos += vector;
            #endregion
        }

        #region physics
        private Vector2 DragVector()
        {
            float dragY = -0.5f * Globals.airDensity * Globals.dragCoeff * (float)Math.Pow(GetCollisionBox().Width * Globals.metersPerPixel, 2) * (float)Math.Pow(vector.Y, 3) / Math.Abs(vector.Y);
            float dragX = -0.5f * Globals.airDensity * Globals.dragCoeff * (float)Math.Pow(GetCollisionBox().Height * Globals.metersPerPixel, 2) * (float)Math.Pow(vector.X, 3) / Math.Abs(vector.X);
            #region check for NaN
            if (float.IsNaN(dragY))
                dragY = 0;
            if (float.IsNaN(dragX))
                dragX = 0;
            #endregion
            return new Vector2(dragX, dragY);
        }
        #endregion
        #region collision methods
        public Rectangle GetCollisionBox()
        {
            return new Rectangle((int)pos.X, (int)pos.Y, (int)dims.X, (int)dims.Y);
        }
        public Vector2 GetSpeed()
        {
            return vector;
        }
        public bool CollisionCheck(CollisionObject obj)
        {
            return this.GetCollisionBox().Intersects(obj.GetCollisionBox());
        }
        #endregion

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
            Globals.spriteBatch.DrawString(Globals.arial, $"{vector}", new Vector2(100, 100), Color.White);
        }
    }
}
