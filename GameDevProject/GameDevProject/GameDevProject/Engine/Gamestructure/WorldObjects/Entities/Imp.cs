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
using GameDevProject.Engine.Gamestructure.WorldObjects.Entities.Projectiles;
using GameDevProject.Engine.PhysicsComponents;

namespace GameDevProject.Engine.Gamestructure.WorldObjects.Entities
{
    class Imp : Sprite, Entity
    {//Walking: 39x53, Attacking: 59x51, Dying: 57x61
        #region variables
        #region animation
        private int animationFrame;
        private int frame;
        private bool lastDirRight = false;
        #endregion
        #region physics
        private Vector2 physicsVector;
        private Vector2 vector = new Vector2(0f, 0f);
        private float mass = 108.86f;
        #endregion
        #region movement
        private bool isFalling = false;
        private float walkingAccelaration = 0.1f;
        private float speed = 1;
        private Vector2 goingTo;
        private Segment walkingSegment;
        #endregion
        #region entity
        public int viewingDistance;
        public bool gotHit = false;
        public int gotHitCounter = 0;
        public bool isAttacking = false;
        public bool allowedToShoot = false;
        public TimeSpan attackingCounter = new TimeSpan(0, 0, 0, 1, 500);
        public TimeSpan cooldown = new TimeSpan(0, 0, 0, 2, 0);
        public float health;
        public float damage;
        private bool dead = false;
        private TimeSpan deathFrame = new TimeSpan(0);
        private List<LostSoul> projectiles = new List<LostSoul>();
        #endregion
        #endregion
        #region Constructors
        public Imp(Vector2 _pos, Vector2 _dims, string _path = "Sprites\\Imp", float _health = 50, float _damage = 50, int _viewingDistance = 200) : base(_path, _pos, _dims)
        {
            health = _health;
            damage = _damage;
            viewingDistance = _viewingDistance;
        }
        #endregion
        #region Sprite
        /// <summary>
        /// Updates the variables of the Imp.
        /// </summary>
        public override void Update()
        {
            if (health <= 0)
                dead = true;
            if(!dead)
                ComputeInput();
            Move();
            UpdateFrame();
            for(int i = 0; i < projectiles.Count; i++)
            {
                projectiles[i].Update();
            }
        }
        /// <summary>
        /// Draws the Imp to the screen.
        /// </summary>
        public override void Draw()
        {
            Color color = Color.White;
            if (gotHit)
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
                        base.Draw(new Rectangle(animationFrame * 39, 0, 39, 53), Vector2.Zero, Vector2.Zero, color, SpriteEffects.FlipHorizontally);
                    }
                    else if (Math.Round(vector.X, 1) < -0.1)
                    {
                        lastDirRight = false;
                        base.Draw(new Rectangle(animationFrame * 39, 0, 39, 53), Vector2.Zero, Vector2.Zero, color);
                    }
                    else
                    {
                        if (lastDirRight)
                            base.Draw(new Rectangle(0, 54, 39, 53), Vector2.Zero, Vector2.Zero, color, SpriteEffects.FlipHorizontally);
                        else
                            base.Draw(new Rectangle(0, 54, 39, 53), Vector2.Zero, Vector2.Zero, color);
                    }
                }
                else
                {
                    frame = 0;
                    if (attackingCounter > cooldown - new TimeSpan(0, 0, 0, 0, 500) - new TimeSpan(0, 0, 0, 0, 500) / 6)
                        frame = (int)((attackingCounter - (cooldown - new TimeSpan(0, 0, 0, 0, 500)) + new TimeSpan(0, 0, 0, 0, 500) / 6) / (new TimeSpan(0, 0, 0, 0, 500) / 3));
                    if (frame > 2)
                        frame = 2;
                    if (lastDirRight)
                        base.Draw(new Rectangle(59 * frame, 55, 59, 51), new Vector2(20, 2), new Vector2(0, -2), color, SpriteEffects.FlipHorizontally);
                    else
                        base.Draw(new Rectangle(59 * frame, 55, 59, 51), new Vector2(20, 2), new Vector2(0, -2), color);
                }
            }
            else
            {
                if (deathFrame / new TimeSpan(0, 0, 0, 0, 250) > 7)
                    base.Draw(new Rectangle(57 * 7, 165, 57, 61), new Vector2(18, 8), new Vector2(0, -8), color);
                else
                    base.Draw(new Rectangle(57 * (int)(deathFrame / new TimeSpan(0, 0, 0, 0, 250)), 165, 57, 61), new Vector2(18, 8), new Vector2(0, -8), color);
            }

            for(int i = 0; i < projectiles.Count; i++)
            {
                projectiles[i].Draw();
            }
        }
        #endregion
        #region Entity
        /// <summary>
        /// Determines what the AI will do based on wether the player is on view and where he is on the platform.
        /// </summary>
        public void ComputeInput()
        {
            if (VisionBox().Intersects(((CollisionObject)Globals.currWorld.hero).GetCollisionBox()) || VisionBox().Contains(((CollisionObject)Globals.currWorld.hero).GetCollisionBox()) || ((CollisionObject)Globals.currWorld.hero).GetCollisionBox().Contains(VisionBox()))
            {
                if (!CollisionEngine.CollisionCheck(Globals.currWorld.levels[Globals.currWorld.currLevel].platforms, new Segment(new Vector2(GetCollisionBox().Center.X, GetCollisionBox().Top), new Vector2(Globals.currWorld.hero.GetCollisionBox().Center.X, Globals.currWorld.hero.GetCollisionBox().Top))))
                {
                    if(!isAttacking)
                        viewingDistance *= 2;
                    if (pos.X > Globals.currWorld.hero.pos.X)
                        lastDirRight = false;
                    else
                        lastDirRight = true;
                    Attack();
                    vector = Vector2.Zero;
                    isAttacking = true;
                }
            }
            else
            {
                if(isAttacking)
                    viewingDistance /= 2;
                isAttacking = false;
                attackingCounter = new TimeSpan(0, 0, 0, 1, 250);
            }
            if(!isAttacking)
            {
                if(physicsVector.Y > 0.1f)
                {
                    walkingSegment = null;
                }
                else
                {
                    if(walkingSegment != null)
                    {
                        if (Math.Sign(physicsVector.X) != Math.Sign(goingTo.X - pos.X) && Math.Sign(physicsVector.X) != 0 && Math.Abs(physicsVector.X) > 0.5)
                        {
                            if (goingTo == walkingSegment.defPoint1)
                                goingTo = walkingSegment.defPoint2;
                            else
                                goingTo = walkingSegment.defPoint1;
                        }
                        if (Math.Abs(goingTo.X - pos.X) < dims.X)
                        {
                            if (goingTo == walkingSegment.defPoint1)
                                goingTo = walkingSegment.defPoint2;
                            else
                                goingTo = walkingSegment.defPoint1;
                        }
                        if (Math.Sign(goingTo.X - pos.X) < 0)
                        {
                            if (vector.X > -speed)
                                vector.X += Math.Sign(goingTo.X - pos.X) * walkingAccelaration;
                        }
                        else
                        {
                            if (vector.X < speed)
                                vector.X += Math.Sign(goingTo.X - pos.X) * walkingAccelaration;
                        }
                    }
                    else
                    {
                        FindNewWalkingSegment();
                        if (walkingSegment != null)
                        {
                            if (lastDirRight)
                            {
                                if (walkingSegment.defPoint1.X > walkingSegment.defPoint2.X)
                                    goingTo = walkingSegment.defPoint1;
                                else
                                    goingTo = walkingSegment.defPoint2;
                            }
                            else
                            {
                                if (walkingSegment.defPoint1.X > walkingSegment.defPoint2.X)
                                    goingTo = walkingSegment.defPoint2;
                                else
                                    goingTo = walkingSegment.defPoint1;
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Determines the visionbox of the AI.
        /// </summary>
        /// <returns>The rectangle in which the AI is able to see the player.</returns>
        public Rectangle VisionBox()
        {
            if(lastDirRight)
            {
                return new Rectangle((int)(pos.X - 50), (int)(pos.Y - viewingDistance), (int)(dims.X + viewingDistance + 50), (int)(dims.Y + 2 * viewingDistance));
            }
            else
            {
                return new Rectangle((int)(pos.X - viewingDistance), (int)(pos.Y - viewingDistance), (int)(dims.X + viewingDistance + 50), (int)(dims.Y + 2 * viewingDistance));
            }
        }
        /// <summary>
        /// Summons a Lost Soul projectile fired in the direction of the player.
        /// </summary>
        public void Attack()
        {
            if(allowedToShoot)
            {
                allowedToShoot = false;
                projectiles.Add(new LostSoul(this, damage, new Vector2(pos.X - Globals.currWorld.hero.pos.X, pos.Y - Globals.currWorld.hero.pos.Y), 3, new TimeSpan(0, 0, 5), pos, new Vector2(67, 33)));
            }
        }
        /// <summary>
        /// This is the method a projectile can call after it dies to be removed from the projectile list of the AI.
        /// </summary>
        /// <param name="projectile">The projectile to remove from the list.</param>
        public void DeleteProjectile(LostSoul projectile)
        {
            bool done = false;
            for(int i = 0; i < projectiles.Count && !done; i++)
            {
                if (projectiles[i] == projectile)
                {
                    projectiles.RemoveAt(i);
                    done = true;
                }
            }
        }
        /// <summary>
        /// Resets the physics vector after adding it's values to the general vector and then updates the position.
        /// </summary>
        public void Move()
        {
            if (dead)
                vector = Vector2.Zero;
            vector += physicsVector;
            physicsVector = Vector2.Zero;
            vector = new Vector2((float)Math.Round(vector.X, 3), (float)Math.Round(vector.Y, 3));
            pos += vector;
        }
        /// <summary>
        /// Updates animation frame counters.
        /// </summary>
        public void UpdateFrame()
        {
            if (gotHit)
                gotHitCounter++;
            if (gotHitCounter > 5)
                gotHit = false;
            if (!dead)
            {
                if (!isAttacking)
                {
                    if (!isFalling)
                    {
                        frame = (int)((frame + 1) % (24 / MathF.Sqrt((float)(Math.Pow(vector.X, 2) + Math.Pow(vector.Y, 2)))));
                        if (frame == 0)
                            animationFrame = (animationFrame + 1) % 4;
                    }
                }
                else
                {
                    attackingCounter += Globals.deltaTime;
                    if (attackingCounter > cooldown)
                    {
                        allowedToShoot = true;
                        attackingCounter = new TimeSpan(0);
                    }
                }
            }
            else if (deathFrame < new TimeSpan(0,0,5))
            {
                deathFrame += Globals.deltaTime;
            }
        }
        /// <summary>
        /// Sets the gotHit boolean to true and lowers the AI's HP.
        /// </summary>
        /// <param name="_damage">The amount with which to decrement the AI's health.</param>
        public void GetHit(float _damage)
        {
            health -= _damage;
            gotHit = true;
            gotHitCounter = 0;
        }
        /// <summary>
        /// This function is called when the AI dies.
        /// </summary>
        public void Die()
        {
            dead = true;
            vector = Vector2.Zero;
        }
        /// <summary>
        /// Clones the entity.
        /// </summary>
        /// <returns>A clone of the entity.</returns>
        public Entity Clone()
        {
            return new Imp(pos, dims, _damage: damage, _viewingDistance: viewingDistance);
        }
        #endregion
        #region AI
        /// <summary>
        /// Finds a new segment to walk on if possible.
        /// </summary>
        public void FindNewWalkingSegment()
        {
            Platform[] possiblePlatforms = FindRightXPlatforms(FindRightYPlatforms(Globals.currWorld.levels[Globals.currWorld.currLevel].platforms));
            if(possiblePlatforms.Length != 0)
                walkingSegment = FindWalkingSegment(possiblePlatforms[0].AIWalkingSegments);
        }
        /// <summary>
        /// Finds platforms that are in the right Y range for the AI to stand on.
        /// </summary>
        /// <param name="platforms">The list of platforms from which to choose.</param>
        /// <returns>A list of platforms that fit the parameters.</returns>
        public Platform[] FindRightYPlatforms(Platform[] platforms)
        {
            List<Platform> output = new List<Platform>();
            foreach(Platform platform in platforms)
            {
                if(Math.Abs(platform.GetCollisionBox().Top - GetCollisionBox().Bottom) <= 10)
                    output.Add(platform);
            }
            return output.ToArray();
        }
        /// <summary>
        /// Finds platforms that are in the right X range for the AI to stand on.
        /// </summary>
        /// <param name="platforms">The list of platforms from which to choose.</param>
        /// <returns>A list of platforms that fit the parameters.</returns>
        public Platform[] FindRightXPlatforms(Platform[] platforms)
        {
            List<Platform> output = new List<Platform>();
            foreach(Platform platform in platforms)
            {
                if ((platform.GetCollisionBox().Left >= ((CollisionObject)this).GetCollisionBox().Left && platform.GetCollisionBox().Left <= ((CollisionObject)this).GetCollisionBox().Right) || (platform.GetCollisionBox().Right >= ((CollisionObject)this).GetCollisionBox().Left && platform.GetCollisionBox().Right <= ((CollisionObject)this).GetCollisionBox().Right) || (platform.GetCollisionBox().Left < ((CollisionObject)this).GetCollisionBox().Left && platform.GetCollisionBox().Right > ((CollisionObject)this).GetCollisionBox().Right))
                    output.Add(platform);
            }
            return output.ToArray();
        }
        /// <summary>
        /// Finds the segment on which to walk from a list of possible segments.
        /// </summary>
        /// <param name="segments">The list of segments from which to choose.</param>
        /// <returns>The chosen segment.</returns>
        public Segment FindWalkingSegment(Segment[] segments)
        {
            List<Segment> tempSegmentSolution = new List<Segment>();
            foreach(Segment segment in segments)
            {
                if ((segment.defPoint1.X > ((CollisionObject)this).GetCollisionBox().Left && segment.defPoint1.X < ((CollisionObject)this).GetCollisionBox().Right) || (segment.defPoint2.X > ((CollisionObject)this).GetCollisionBox().Left && segment.defPoint2.X < ((CollisionObject)this).GetCollisionBox().Right) || (segment.defPoint1.X < ((CollisionObject)this).GetCollisionBox().Left && segment.defPoint2.X > ((CollisionObject)this).GetCollisionBox().Right) || (segment.defPoint2.X < ((CollisionObject)this).GetCollisionBox().Left && segment.defPoint1.X > ((CollisionObject)this).GetCollisionBox().Right))
                    tempSegmentSolution.Add(segment);
            }
            if(tempSegmentSolution.Count > 1)
            {
                if(tempSegmentSolution[0].defPoint1.X > ((CollisionObject)this).GetCollisionBox().Left && tempSegmentSolution[0].defPoint2.X > ((CollisionObject)this).GetCollisionBox().Left)
                {
                    if (lastDirRight)
                        return tempSegmentSolution[0];
                    else
                        return tempSegmentSolution[1];
                }
                else
                {
                    if (lastDirRight)
                        return tempSegmentSolution[1];
                    else
                        return tempSegmentSolution[0];
                }
            }
            else if(tempSegmentSolution.Count != 0)
            {
                return tempSegmentSolution[0];
            }
            return null;
        }
        #endregion
        #region CollisionObject
        public Rectangle GetCollisionBox()
        {
            if (!dead)
                return new Rectangle((int)pos.X, (int)pos.Y, (int)dims.X, (int)dims.Y);
            else
                return Rectangle.Empty;
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
            return dead;
        }
        #endregion
    }
}