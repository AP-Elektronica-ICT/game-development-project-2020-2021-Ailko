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
using GameDevProject.Engine.Geometry;
using GameDevProject.Engine.PhysicsComponents;
using GameDevProject.UI;

namespace GameDevProject.Engine.Gamestructure.WorldObjects.Entities
{
    public class Hero : Sprite, Entity
    {
        // walking width = 41 height = 54, shooting width = 52 height = 53, death width = 57 height = 59
        #region variables
        #region animation
        private int animationFrame;
        private int frame;
        private bool lastDirRight = false;
        #endregion
        #region physics
        private Vector2 physicsVector;
        private Vector2 vector = new Vector2(0f, 0f);
        private float mass = 163.29f;
        #endregion
        #region movement
        private bool hasJumped = false;
        private float walkingAccelaration = 0.1f;
        private float walkingAccelarationMultiplier = 1f;
        private float speed = 2;
        private float jumpForce = 3f;
        #endregion
        #region entity
        public bool gotHit = false;
        public int gotHitCounter = 0;
        public bool isAttacking = false;
        public TimeSpan attackCooldown = new TimeSpan(0, 0, 0, 0, 200);
        public TimeSpan attackingCounter = new TimeSpan(0);
        public float health;
        public float damage;
        private bool dead = false;
        private TimeSpan deathFrame = new TimeSpan(0);
        #endregion
        #endregion
        #region Constructors
        public Hero(Vector2 _pos, Vector2 _dims, string _path = "Sprites\\DoomGuy") : base(_path, _pos, _dims)
        {
            animationFrame = 0;
            frame = 0;
            health = 100;
            damage = 10;
        }
        #endregion
        #region Sprite
        public override void Update()
        {
            if (pos.X > Globals.currWorld.levels[Globals.currWorld.currLevel].endOfLevelX)
                Globals.currWorld.levels[Globals.currWorld.currLevel].finished = true;
            if (health <= 0)
            {
                Die();
                if (health < 0)
                    health = 0;
            }
            if(!dead)
                ComputeInput();
            else
                vector.X -= Math.Sign(vector.X) * walkingAccelaration / 2;
            Move();
            UpdateFrame();
        }
        public override void Draw()
        {
            Color color = Color.White;
            if(gotHit)
            {
                color = Color.Red;
            }
            if (!dead)
            {
                if (!isAttacking)
                {
                    if (Math.Round(vector.X, 1) > 0.1)
                    {
                        lastDirRight = true;
                        base.Draw(new Rectangle(animationFrame * 41, 0, 41, 54), Vector2.Zero, Vector2.Zero, color, SpriteEffects.FlipHorizontally);
                    }
                    else if (Math.Round(vector.X, 1) < -0.1)
                    {
                        lastDirRight = false;
                        base.Draw(new Rectangle(animationFrame * 41, 0, 41, 54), Vector2.Zero, Vector2.Zero, color);
                    }
                    else
                    {
                        if (lastDirRight)
                            base.Draw(new Rectangle(0, 54, 52, 53), new Vector2(11, -1), Vector2.Zero, color, SpriteEffects.FlipHorizontally);
                        else
                            base.Draw(new Rectangle(0, 54, 52, 53), new Vector2(11, -1), Vector2.Zero, color);
                    }
                }
                else
                {
                    if (attackingCounter > attackCooldown / 2)
                    {
                        if (lastDirRight)
                            base.Draw(new Rectangle(0, 54, 52, 53), new Vector2(11, -1), Vector2.Zero, color, SpriteEffects.FlipHorizontally);
                        else
                            base.Draw(new Rectangle(0, 54, 52, 53), new Vector2(11, -1), Vector2.Zero, color);
                    }
                    else
                    {
                        if (lastDirRight)
                            base.Draw(new Rectangle(53, 54, 52, 53), new Vector2(11, -1), Vector2.Zero, color, SpriteEffects.FlipHorizontally);
                        else
                            base.Draw(new Rectangle(53, 54, 52, 53), new Vector2(11, -1), Vector2.Zero, color);
                    }
                }
            }
            else
            {
                if (deathFrame / new TimeSpan(0, 0, 0, 0, 250) > 9)
                    base.Draw(new Rectangle(57 * 9, 107, 57, 59), new Vector2(16, 5), new Vector2(0, -5), color);
                else
                    base.Draw(new Rectangle(57 * (int)(deathFrame / new TimeSpan(0, 0, 0, 0, 250)), 107, 57, 59), new Vector2(16, 5), new Vector2(0, -5), color);
            }
        }
        #endregion
        #region Entity
        public void ComputeInput()
        {
            Input.Action[] actions = Globals.input.ActionsPressed();
            #region walking
            bool walked = false;
            if (!isAttacking)
            {
                if (actions.Contains(Input.Action.Left))
                {
                    walked = true;
                    if (vector.X > -speed)
                        vector.X -= walkingAccelaration * walkingAccelarationMultiplier;
                }
                if (actions.Contains(Input.Action.Right))
                {
                    walked = true;
                    if (vector.X < speed)
                        vector.X += walkingAccelaration * walkingAccelarationMultiplier;
                }
            }
            if (!walked)
            {
                vector.X -= Math.Sign(vector.X) * walkingAccelaration / 2;
            }
            #endregion
            #region jumping
            #region check in air
            if (physicsVector.Y < 0)
            {
                hasJumped = false;
                walkingAccelarationMultiplier = 1f;
            }
            else
            {
                hasJumped = true;
                walkingAccelarationMultiplier = 0.5f;
            }
            #endregion
            if (!hasJumped && actions.Contains(Input.Action.Jump))
            {
                hasJumped = true;
                vector.Y -= jumpForce;
                walkingAccelarationMultiplier = 0.5f;
            }
            #endregion
            #region attacking
            if (actions.Contains(Input.Action.Shoot))
                Attack();
            else
            {
                attackingCounter = new TimeSpan(0);
                isAttacking = false;
            }
            #endregion
        }
        public void Move()
        {
            vector += physicsVector;
            physicsVector = Vector2.Zero;
            vector = new Vector2((float)Math.Round(vector.X, 3), (float)Math.Round(vector.Y, 3));
            pos += vector;
        }
        public void UpdateFrame()
        {
            if(gotHit)
                gotHitCounter++;
            if (gotHitCounter > 5)
                gotHit = false;
            if (!dead)
            {
                if (!isAttacking)
                {
                    if (!hasJumped)
                    {
                        frame = (int)((frame + 1) % (24 / MathF.Sqrt((float)(Math.Pow(vector.X, 2) + Math.Pow(vector.Y, 2)))));
                        if (frame == 0)
                            animationFrame = (animationFrame + 1) % 4;
                    }
                }
                else
                {
                    attackingCounter += Globals.deltaTime;
                    if (attackingCounter > attackCooldown)
                        attackingCounter = new TimeSpan(0);
                }
            }
            else if (deathFrame < new TimeSpan(0,0,5))
            {
                deathFrame += Globals.deltaTime;
            }
        }
        public void Attack()
        {
            isAttacking = true;
            if(attackingCounter == new TimeSpan(0))
            {
                CollisionObject[] entities = GetClosestTwoX(ObjOnY(Globals.currWorld.levels[Globals.currWorld.currLevel].entities, GetCollisionBox().Center.Y), GetCollisionBox().Center.X);
                if(lastDirRight)
                {
                    if (entities[1] != null && !CollisionEngine.CollisionCheck(Globals.currWorld.levels[Globals.currWorld.currLevel].platforms, new Segment(new Vector2(GetCollisionBox().Center.X, GetCollisionBox().Center.Y), new Vector2(entities[1].GetCollisionBox().Center.X, entities[1].GetCollisionBox().Center.Y))))
                        ((Entity)entities[1]).GetHit(damage);
                }
                else
                {
                    if (entities[0] != null && !CollisionEngine.CollisionCheck(Globals.currWorld.levels[Globals.currWorld.currLevel].platforms, new Segment(new Vector2(GetCollisionBox().Center.X, GetCollisionBox().Center.Y), new Vector2(entities[0].GetCollisionBox().Center.X, entities[0].GetCollisionBox().Center.Y))))
                        ((Entity)entities[0]).GetHit(damage);
                }
            }
        }
        public CollisionObject[] ObjOnY(CollisionObject[] objects, float YCoord)
        {
            List<CollisionObject> output = new List<CollisionObject>();
            foreach(CollisionObject obj in objects)
            {
                if(obj != this)
                    if (obj.GetCollisionBox().Top <= YCoord && obj.GetCollisionBox().Bottom >= YCoord)
                        output.Add(obj);
            }
            return output.ToArray();
        }
        public CollisionObject[] ObjBetweenXs(float minX, float maxX, CollisionObject[] objects)
        {
            List<CollisionObject> output = new List<CollisionObject>();
            foreach (CollisionObject obj in objects)
            {
                if ((obj.GetCollisionBox().Left >= minX && obj.GetCollisionBox().Left <= maxX) || (obj.GetCollisionBox().Right >= minX && obj.GetCollisionBox().Right <= maxX) || (obj.GetCollisionBox().Left < minX && obj.GetCollisionBox().Left > maxX))
                    output.Add(obj);
            }
            return output.ToArray();
        }
        public CollisionObject[] GetClosestTwoX(CollisionObject[] objects, float X)
        {
            CollisionObject[] closest = new CollisionObject[2];
            foreach(CollisionObject obj in objects)
            {
                if(obj.GetCollisionBox().X > X)
                {
                    if (closest[1] == null || obj.GetCollisionBox().X < closest[1].GetCollisionBox().X)
                        closest[1] = obj;
                }
                else
                {
                    if (closest[0] == null || obj.GetCollisionBox().X > closest[0].GetCollisionBox().X)
                        closest[0] = obj;
                }
            }
            return closest;
        }
        public void GetHit(float _damage)
        {
            health -= _damage;
            gotHit = true;
            gotHitCounter = 0;
        }
        public void Die()
        {
            Globals.UI.dead = true;
            dead = true;
        }
        public Entity Clone()
        {
            return new Hero(pos, dims);
        }
        #endregion
        #region CollisionObject
        public Rectangle GetCollisionBox()
        {
            return new Rectangle((int)pos.X, (int)pos.Y, (int)dims.X, (int)dims.Y);
        }
        public Vector2 GetSpeed()
        {
            return vector;
        }
        public Vector2 GetSpeedTotal()
        {
            return vector + physicsVector;
        }
        public float GetMass()
        {
            return mass;
        }
        public void SavePhysicsVector(Vector2 vector)
        {
            physicsVector += vector;
        }
        public bool IsStatic()
        {
            return false;
        }
        public float CollisionImpactMultiplier()
        {
            return 1f;
        }
        #endregion
    }
}