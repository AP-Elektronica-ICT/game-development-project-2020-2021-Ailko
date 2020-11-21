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
    public class Hero : Sprite
    {
        private float speed = 2;
        private float jumpForce = 7.5f;

        private Vector2 vector = new Vector2(0f, 0f);

        private bool hasJumped = false;
        private float jumpedFromY = 0;
        public Hero(string _path, Vector2 _pos, Vector2 _dims) : base(_path, _pos, _dims)
        {

        }

        public override void Update()
        {
            if(Globals.keyboard.GetPress("Q"))
            {
                if (vector.X > -speed)
                    vector.X -= 0.1f;
            }
            else if (Globals.keyboard.GetPress("D"))
            {
                if (vector.X < speed)
                    vector.X += 0.1f;
            }
            else
            {
                vector.X -= vector.X / 20;
            }
            if ((Globals.keyboard.GetPress("Z") || Globals.keyboard.GetPress("Space")) && !hasJumped)
            {
                hasJumped = true;
                jumpedFromY = pos.Y;
                vector.Y -= jumpForce;
            }
            else if(pos.Y < jumpedFromY)
            {
                vector.Y += Globals.gravity * (float)Globals.deltaTime.TotalSeconds;
            }
            else
            {
                vector.Y = 0;
                hasJumped = false;
            }

            pos = new Vector2(pos.X + vector.X, pos.Y + vector.Y);

            base.Update();
        }

        public override void Draw()
        {
            base.Draw();
        }
    }
}
