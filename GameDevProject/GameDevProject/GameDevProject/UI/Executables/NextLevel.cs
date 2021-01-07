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
using GameDevProject.UI.Interfaces;

namespace GameDevProject.UI.Executables
{
    public class NextLevel : Executable
    {
        public void Execute()
        {
            Globals.currWorld.levels[Globals.currWorld.currLevel].EntityReset();
            Globals.currWorld.currLevel++;
            Globals.currWorld.levels[Globals.currWorld.currLevel].EntityReset();
            Globals.currWorld.levels[Globals.currWorld.currLevel].StartLevel();
        }
    }
}
