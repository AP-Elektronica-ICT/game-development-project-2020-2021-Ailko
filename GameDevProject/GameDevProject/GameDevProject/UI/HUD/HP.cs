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

namespace GameDevProject.UI.HUD
{
    class HP : IHUD
    {
        #region variables
        private float HealthPoints = 0;
        private float HealthPointsMax = 0;
        #endregion

        #region constructors
        public HP(Vector2 _pos) : base(_pos)
        {
            HealthPointsMax = Globals.currWorld.hero.health;
        }
        #endregion
        #region IHUD
        public override void Update()
        {
            HealthPoints = Globals.currWorld.hero.health;
        }
        public override void Draw()
        {
            base.Draw($"{HealthPoints}/{HealthPointsMax}", new Vector2(-3 * 10, 0), Color.Red);
        }
        #endregion
    }
}
