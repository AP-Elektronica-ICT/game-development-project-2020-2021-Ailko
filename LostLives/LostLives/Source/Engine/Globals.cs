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
        public static ContentManager content;
        public static SpriteBatch spriteBatch;

        public static LLKeyboard keyboard;

        public static DateTime lastFrame;
        public static TimeSpan deltaTime;

        public static float gravity = 9.81f;
    }
}
