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
using GameDevProject.Engine.PhysicsComponents;
using GameDevProject.Engine.Interfaces;

namespace GameDevProject.Engine.Gamestructure.WorldObjects.Entities.Projectiles
{
    class LostSoul : Sprite
    {
        #region Variables
        public float damage;
        public float speed;
        public Vector2 direction;
        public float rotationSprite;
        public bool flipSprite;
        public DateTime timeCreated;
        public TimeSpan timeToLive;
        public Imp parent;
        #endregion

        #region constructor
        public LostSoul(Imp _parent, float _damage, Vector2 _direction, float _speed, TimeSpan _timeToLive, Vector2 _pos, Vector2 _dims, string _path = "Sprites\\Lost Soul") : base(_path, _pos, _dims)
        {
            damage = _damage;
            parent = _parent;
            direction = _direction;
            direction.Normalize();
            speed = _speed;

            if (direction.X > 0)
                flipSprite = true;

            rotationSprite = (-1 * direction.X + 0 * _direction.Y) / (new Vector2(-1, 0).Length() * _direction.Length());

            timeCreated = DateTime.Now;
            timeToLive = _timeToLive;
        }
        #endregion
        #region Sprite
        public override void Update()
        {
            CheckAliveTime();
            CheckCollision();
            Move();
        }
        public override void Draw()
        {
            if (flipSprite)
                base.Draw(new Rectangle(0, 0, 67, 33), rotationSprite, SpriteEffects.FlipHorizontally);
            else
                base.Draw(new Rectangle(0, 0, 67, 33), rotationSprite);
        }
        #endregion
        #region Lost Soul
        #region checks
        public void CheckAliveTime()
        {
            if(DateTime.Now - timeCreated > timeToLive)
            {
                parent.DeleteProjectile(this);
            }
        }
        public void CheckCollision()
        {
            if (CollisionEngine.CollisionCheck(new Rectangle((int)(pos.X - dims.X / 2), (int)(pos.Y - dims.Y / 2), (int)dims.X, (int)dims.Y), Globals.currWorld.levels[Globals.currWorld.currLevel].platforms))
            {
                parent.DeleteProjectile(this);
            }
            if(CollisionEngine.CollisionCheck(new Rectangle((int)pos.X, (int)pos.Y, (int)dims.X, (int)dims.Y), ((CollisionObject)Globals.currWorld.hero).GetCollisionBox()))
            {
                ((Entity)Globals.currWorld.hero).GetHit(damage);
                parent.DeleteProjectile(this);
            }
        }
        #endregion
        public void Move()
        {
            pos -= direction * speed;
        }
        #endregion
    }
}