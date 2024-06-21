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
        public IPickable PickableOnTile { get; private set; }

        private Player _owner;
        [SerializeField] MeshRenderer _meshRenderer;
        [SerializeField] Material _defaultTileMat;

        public void SetCoordinate(int z, int x)
        {
            X = x;
            Z = z;
        }

        public void SetOwner(Player player, bool withAnim = false)
        {
            _owner = player;
            if (player != null)
            {
                _meshRenderer.material = player.associatedColorMaterial;
            }
            else
            {
                _meshRenderer.material = _defaultTileMat;
            }
        }

        public void SetOccupiedByPlayer(bool occupied)
        {
            OccupiedByPlayer = occupied;
        }

        public void SetOccupiedByObject(bool occupied, IPickable objectOnTile)
        {
            OccupiedByObject = occupied;
            PickableOnTile = objectOnTile;
        }

        public Player GetOwner()
        {
            return _owner;
        }
    }
}
