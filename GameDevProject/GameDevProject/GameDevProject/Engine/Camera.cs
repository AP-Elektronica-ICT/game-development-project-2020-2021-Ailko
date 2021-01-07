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

namespace GameDevProject.Engine
{
    public class Camera
    {
        #region variables
        public Matrix transform;
        public Matrix transformBG;
        #endregion

        public void Follow(Sprite _target)
        {
            #region cameraMovement
            Matrix offset = Matrix.CreateTranslation
                (
                    Globals.screenSize.X / 2,
                    Globals.screenSize.Y / 2,
                    0
                );
            Matrix position = Matrix.CreateTranslation
                (
                    -_target.pos.X - (_target.dims.X / 2),
                    -_target.pos.Y - (_target.dims.Y / 2),
                    0
                );
            Matrix zoom = Matrix.CreateScale(1, 1, 1);
            transform = position * offset * zoom;
            #endregion
            #region BG movement
            Matrix zoomBG = Matrix.CreateScale(1.25f);
            Matrix offsetBG = Matrix.CreateTranslation
                (
                    Globals.screenSize.X / 2,
                    Globals.screenSize.Y / 3.5f,
                    0
                );
            Matrix positionBG = Matrix.CreateTranslation
                (
                    -_target.pos.X - (_target.dims.X / 2),
                    -(_target.pos.Y + (_target.dims.Y / 2)) * 0.75f,
                    0
                );
            transformBG = positionBG * offsetBG * zoomBG;
            #endregion
            #region BG pos update
            Vector2 posUpd = new Vector2((int)_target.pos.X / 800, (int)_target.pos.Y / 500);
            for (int y = -1; y < 2; y++)
            {
                for (int x = -1; x < 2; x++)
                {
                    Globals.bgPos[y + 1, x + 1] = new Vector2(posUpd.X + x, posUpd.Y + y);
                }
            }
            #endregion
        }
    }
}
