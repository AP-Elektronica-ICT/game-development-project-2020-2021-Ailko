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
using GameDevProject.Engine.Gamestructure;
using GameDevProject.Engine.Input;
using GameDevProject.Engine;
using GameDevProject.UI;

namespace GameDevProject
{
    public static class Globals
    {
        #region drawing
        public static ContentManager content;
        public static SpriteBatch spriteBatch;
        public static SpriteBatch spriteBatch2;

        public static Vector2 screenSize;

        public static Camera camera;

        public static Vector2[,] bgPos;
        #endregion
        #region backgrounds
        public static Texture2D bg1;
        public static Texture2D bg2;
        #endregion
        #region fonts
        public static SpriteFont arial;
        #endregion
        #region input
        public static Input input;
        #endregion
        #region timing frame
        public static DateTime lastFrame;
        public static TimeSpan deltaTime;
        #endregion
        public static Random rng;
        public static World currWorld;
        public static UIParent UI;
        public static Game game;
    }
}
