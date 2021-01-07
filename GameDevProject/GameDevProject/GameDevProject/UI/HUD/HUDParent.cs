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

namespace GameDevProject.UI.HUD
{
    public class HUDParent : IHUD
    {
        #region variables
        public IHUD[] HUDelements;
        #endregion

        #region constructors
        public HUDParent() : base(new Vector2(0, 0))
        {
            HUDelements = new IHUD[]
            {
                new HP(new Vector2(Globals.screenSize.X / 2, 10))
            };
        }
        #endregion
        #region IHUD
        public override void Update()
        {
            foreach(IHUD hud in HUDelements)
            {
                hud.Update();
            }
        }
        public override void Draw()
        {
            foreach(IHUD hud in HUDelements)
            {
                hud.Draw();
            }
        }
        #endregion
    }
}
