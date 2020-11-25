﻿#region Includes
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
    class Line
    {
        public Vector2 multipliers;
        public float constant;

        public Line(Vector2 _multipliers, float _constant = 0)
        {
            multipliers = _multipliers;
            constant = _constant;
        }
        public Line(Vector2 pos1, Vector2 pos2)
        {
            float slope = (pos2.Y - pos1.Y) / (pos2.X - pos1.X);
            multipliers = new Vector2(slope, 1);
            constant = -slope * pos1.X + pos1.Y;
        }

        #region get slope intercept formula
        public float GetSlopeIntercept()
        {
            return -multipliers.X;
        }
        public float GetSlopeInterceptConstant()
        {
            return -constant / multipliers.Y;
        }
        #endregion
        public Vector2 Intersect(Line line)
        {
            float intersectX = (line.GetSlopeInterceptConstant() - GetSlopeInterceptConstant()) / (GetSlopeIntercept() - line.GetSlopeIntercept());
            return new Vector2(intersectX, GetSlopeIntercept() * intersectX + GetSlopeInterceptConstant());
        }
    }
}
