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

namespace GameDevProject.Engine.Geometry
{
    public class Segment
    {
        #region variables
        public Vector2 defPoint1;
        public Vector2 defPoint2;
        #endregion

        #region constructors
        public Segment(Vector2 p1, Vector2 p2)
        {
            defPoint1 = p1;
            defPoint2 = p2;
        }
        #endregion
        #region methods
        public bool Contains(Vector2 p)
        {
            if (((p.Y > defPoint1.Y && p.Y < defPoint2.Y) || (p.Y < defPoint1.Y && p.Y > defPoint2.Y)) && ((p.X > defPoint1.X && p.X < defPoint2.X) || (p.X < defPoint1.X && p.X > defPoint2.X)))
            {
                if (new Line(defPoint1, defPoint2).Contains(p))
                    return true;
            }
            return false;
        }
        public Line ConvertToLine()
        {
            return new Line(defPoint1, defPoint2);
        }
        #endregion
    }
}
