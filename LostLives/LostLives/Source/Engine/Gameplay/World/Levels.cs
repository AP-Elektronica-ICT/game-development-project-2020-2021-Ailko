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
    public class Levels
    {
        public Level[] levels;
        public  int currLevel = 0;

        public Levels(Level[] _levels)
        {
            levels = _levels;
        }

        public Level GetCurrLevel()
        {
            return levels[currLevel];
        }

        public void Draw()
        {
            levels[currLevel].Draw();
        }
    }
}
