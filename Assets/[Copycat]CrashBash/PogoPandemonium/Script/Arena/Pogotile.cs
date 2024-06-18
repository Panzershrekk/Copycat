using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PangoPandemonium
{
    [Serializable]
    public class Pogotile : MonoBehaviour
    {
        public int X;
        public int Z;

        public void SetCoordinate(int z, int x)
        {
            X = x;
            Z = z;
        }
    }
}
