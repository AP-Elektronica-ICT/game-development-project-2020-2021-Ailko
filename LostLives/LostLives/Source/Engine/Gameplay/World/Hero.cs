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

        private float speed = 2;
        private float jumpForce = 7f;
        private float mass = 81.63f;

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

            float drag = -0.5f * Globals.airDensity * 0.7f * 0.2f * (float)Math.Pow(vector.Y, 3) / Math.Abs(vector.Y);
            if (float.IsNaN(drag))
                drag = 0;
            float gravitationalAcceleration = Globals.gravity;

            vector.Y += (gravitationalAcceleration + drag) * (float)Globals.deltaTime.TotalSeconds * (54 / 182.88f);

            Vector2 collisionVector = Globals.currWorld.levels.GetCurrLevel().CollisionSpecifics(this);

            if (collisionVector.Y < 0)
                hasJumped = false;

            if ((Globals.keyboard.GetPress("Z") || Globals.keyboard.GetPress("Space") || Globals.keyboard.GetPress("Up")) && !hasJumped)
            {
                hasJumped = true;
                jumpedFromY = pos.Y;
                vector.Y -= jumpForce;
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

            pos = new Vector2(pos.X + vector.X, pos.Y + vector.Y);

            /*if(pos.Y > 400)
            {
                pos.Y = 400;
                vector.Y = 0;
            }*/
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
            Globals.spriteBatch.DrawString(Globals.arial, $"{vector}", new Vector2(100, 100), Color.Black);
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
