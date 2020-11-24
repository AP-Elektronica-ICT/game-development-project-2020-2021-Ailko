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
    public class Hero : Sprite, CollisionObject
    {   // walking width = 41 height = 54, shooting width = 52 height = 53, death width = 57
        private int animationFrame;
        private int frame;

        #region physics constants
        private float speed = 2;
        private float walkingAccelaration = 0.1f;
        private float walkingAccelarationMultiplier = 1f;
        private float jumpForce = 3f;
        private float mass = 163.29f;
        #endregion

        private Vector2 vector = new Vector2(0f, 0f);

        private bool hasJumped = false;

        private bool lastDirRight = false;

        public Hero(string _path, Vector2 _pos, Vector2 _dims) : base(_path, _pos, _dims)
        {
            animationFrame = 0;
            frame = 0;
        }

        public override void Update()
        {
            #region Input horizontal movement
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
            #endregion

            Vector2 drag = DragVector();
            drag /= mass;
            Vector2 gravitationalAcceleration = new Vector2(0, Globals.gravity * mass);

            vector += (drag + gravitationalAcceleration) * (float)Globals.deltaTime.TotalSeconds * Globals.metersPerPixel;

            Vector2 collisionVector = Globals.currWorld.levels.GetCurrLevel().CollisionSpecifics(this);

            #region CheckInAir
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
            if (!hasJumped)
            {
                frame = (int)((frame + 1) % (24 / MathF.Sqrt((float)(Math.Pow(vector.X, 2) + Math.Pow(vector.Y, 2)))));
                if (frame == 0)
                    animationFrame = (animationFrame + 1) % 4;
            }

            vector += collisionVector;

            vector.X = (float)Math.Round(vector.X, 3);
            vector.Y = (float)Math.Round(vector.Y, 3);

            pos += vector;
        }

        private Vector2 DragVector()
        {
            float dragY = -0.5f * Globals.airDensity * Globals.dragCoeff * (float)Math.Pow(GetCollisionBox().Width * Globals.metersPerPixel, 2) * (float)Math.Pow(vector.Y, 3) / Math.Abs(vector.Y);
            float dragX = -0.5f * Globals.airDensity * Globals.dragCoeff * (float)Math.Pow(GetCollisionBox().Height * Globals.metersPerPixel, 2) * (float)Math.Pow(vector.X, 3) / Math.Abs(vector.X);

            if (float.IsNaN(dragY))
                dragY = 0;
            if (float.IsNaN(dragX))
                dragX = 0;

            return new Vector2(dragX, dragY);
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
            Globals.spriteBatch.DrawString(Globals.arial, $"{vector}", new Vector2(100, 100), Color.White);
        }

        private float GetTotalSpeed()
        {
            return (float)Math.Sqrt(Math.Pow(vector.X, 2) + Math.Pow(vector.Y, 2));
        }

        public Vector2 GetSpeed()
        {
            return vector;
        }
    }
}
