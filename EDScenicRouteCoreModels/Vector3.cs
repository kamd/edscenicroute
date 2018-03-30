using System;
using System.Collections.Generic;
using System.Text;

namespace EDScenicRouteCoreModels
{
    public struct Vector3
    {
        public Vector3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public static Vector3 Zero => new Vector3() {X = 0f, Y = 0f, Z = 0f};
    }
}
