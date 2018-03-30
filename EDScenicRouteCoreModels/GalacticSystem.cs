﻿using System;
using System.Numerics;

namespace EDScenicRouteCoreModels
{
    [Serializable]
    public class GalacticSystem : IGalacticPoint
    {
        public string Name { get; set; }
        public Vector3 Coordinates { get; set; }
        public string GalMapUrl { get; set; }



        public override string ToString()
        {
            return "System: Name = " + Name;
        }

        public static readonly GalacticSystem SOL = new GalacticSystem() { Name = "Sol", Coordinates = Vector3.Zero };
    }
}