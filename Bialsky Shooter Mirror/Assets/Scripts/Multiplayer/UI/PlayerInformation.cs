using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.Multiplayer
{
    [Serializable]
    public class PlayerInformation
    {
        public string name;
        public string Name { get { return name; } }

        public PlayerInformation() { }

        public PlayerInformation(string name)
        {
            this.name = name;
        }
    }
}
