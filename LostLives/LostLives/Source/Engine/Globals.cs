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
    public class Globals
    {
        #region drawing
        public static ContentManager content;
        public static SpriteBatch spriteBatch;
        public static Vector2 screenSize;
        #endregion
        #region backgrounds
        public static Texture2D bg1;
        public static Texture2D bg2;
        #endregion
        #region fonts
        public static SpriteFont arial;
        #endregion
        #region input
        public static LLKeyboard keyboard;
        #endregion
        #region timing frame
        public static DateTime lastFrame;
        public static TimeSpan deltaTime;
        #endregion
        #region physics constants
        public static float metersPerPixel = 1.8288f / 54;

        public static float gravity = 2 * 9.81f * metersPerPixel;
        public static float airDensity = 1.225f;
        public static float dragCoeff = 1f;

        public static float cornerRadius = 25f;
        #endregion
        public static Random rng;
        public static World currWorld;
    }
}
