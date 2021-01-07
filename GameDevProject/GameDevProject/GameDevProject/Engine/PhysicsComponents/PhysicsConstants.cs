using System;
using System.Collections.Generic;
using System.Text;

namespace GameDevProject.Engine.PhysicsComponents
{
    public static class PhysicsConstants
    {
        public static float metersPerPixel = 1.8288f / 54;

        public static float gravity = 2 * 9.81f * metersPerPixel;
        public static float airDensity = 1.225f;
        public static float dragCoeff = 1f;

        public static float cornerRadius = 7.5f;
    }
}
