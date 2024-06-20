using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PogoPandemonium
{
    [Serializable]
    public class Pogotile : MonoBehaviour
    {
        public int X;
        public int Z;
        public bool OccupiedByPlayer { get; private set; }
        public bool OccupiedByObject { get; private set; }

        private Player _owner;
        [SerializeField] MeshRenderer _meshRenderer;
        public void SetCoordinate(int z, int x)
        {
            X = x;
            Z = z;
        }

        public void SetOwner(Player player)
        {
            _owner = player;
            _meshRenderer.material = player.associatedColorMaterial;
        }

        public void SetOccupiedByPlayer(bool occupied)
        {
            OccupiedByPlayer = occupied;
        }

        public void SetOccupiedByObject(bool occupied)
        {
            OccupiedByObject = occupied;
        }
    }
}
