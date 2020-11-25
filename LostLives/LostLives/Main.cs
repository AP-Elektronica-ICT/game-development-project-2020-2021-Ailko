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
    public class Main : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        World world;

        public Main()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }
        #region loading and initializing
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }
        protected override void LoadContent()
        {
            Globals.content = this.Content;
            Globals.spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            Globals.keyboard = new LLKeyboard();

            Globals.rng = new Random();

            world = new World();

            Globals.arial = Globals.content.Load<SpriteFont>("Fonts\\Arial16");

            Globals.bg1 = Globals.content.Load<Texture2D>("Backgrounds\\background1");
            Globals.bg2 = Globals.content.Load<Texture2D>("Backgrounds\\background2");

            Globals.currWorld = world;
            Globals.lastFrame = DateTime.Now;
        }
        #endregion
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            // TODO: Add your update logic here

            Globals.keyboard.Update();

            world.Update();

            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            Globals.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

            Globals.spriteBatch.Draw(Globals.bg1, new Rectangle(0, 0, 800, 500), Color.White);

            world.Draw();

            Globals.spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
