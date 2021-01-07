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
using GameDevProject.Engine;
using GameDevProject.Engine.Input;
using GameDevProject.Engine.Gamestructure;

namespace GameDevProject
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();
        }

        protected override void LoadContent()
        {
            Globals.screenSize = new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
            Globals.content = this.Content;
            Globals.spriteBatch = new SpriteBatch(GraphicsDevice);
            Globals.spriteBatch2 = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            Globals.input = new LLMouseAndKeyboard();

            Globals.rng = new Random();

            Globals.arial = Globals.content.Load<SpriteFont>("Fonts\\Arial16");

            Globals.bg1 = Globals.content.Load<Texture2D>("Backgrounds\\background1");
            Globals.bg2 = Globals.content.Load<Texture2D>("Backgrounds\\background2");

            Globals.currWorld = new World();
            Globals.UI = new UI.UIParent();
            Globals.lastFrame = DateTime.Now;

            Globals.game = this;

            Globals.camera = new Camera();
            Globals.bgPos = new Vector2[,] { { new Vector2(-1, -1), new Vector2(0, -1), new Vector2(1, -1) }, { new Vector2(-1, 0), new Vector2(0, 0), new Vector2(1, 0) }, { new Vector2(-1, 1), new Vector2(0, 1), new Vector2(1, 1) } };
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            Globals.input.Update();

            PhysicsEngine.Update();

            Globals.UI.Update();
            Globals.currWorld.Update();

            Globals.camera.Follow(Globals.currWorld.hero);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            Globals.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, transformMatrix: Globals.camera.transformBG);

            for (int y = 0; y < 3; y++)
            {
                for (int x = 0; x < 3; x++)
                {
                    Globals.spriteBatch.Draw(Globals.bg1, new Rectangle(800 * (int)Globals.bgPos[y, x].X, 500 * (int)Globals.bgPos[y, x].Y, 800, 500), Color.White);
                }
            }

            Globals.spriteBatch.End();

            Globals.spriteBatch.Begin(transformMatrix: Globals.camera.transform);

            Globals.currWorld.Draw();

            Globals.spriteBatch.End();


            Globals.spriteBatch2.Begin();

            Globals.UI.Draw();

            Globals.spriteBatch2.End();

            base.Draw(gameTime);
        }
    }
}
