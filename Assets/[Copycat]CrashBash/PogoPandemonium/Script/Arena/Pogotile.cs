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

        private Player _previousOwner;
        private Player _owner;
        [SerializeField] MeshRenderer _meshRenderer;
        [SerializeField] Material _defaultTileMat;

        private float _timeBetweenBlink = GameConstant.TIME_BETWEEN_BLINK;
        private float _blinkTime = GameConstant.BLINK_TIME;

        private bool _tileImmune = false;
        private float _currentBlinkTime = 0f;
        private float _currentBlinkInterval = 0;
        private bool _colorInverter = false;

        private void Update()
        {
            if (_tileImmune == true)
            {
                _currentBlinkTime -= Time.deltaTime;
                _currentBlinkInterval -= Time.deltaTime;
                if (_currentBlinkInterval < 0)
                {
                    if (_colorInverter == false)
                    {
                        _meshRenderer.material = _owner == null ? _defaultTileMat : _owner.associatedColorMaterial;
                    }
                    else
                    {
                        _meshRenderer.material = _previousOwner == null ? _defaultTileMat : _previousOwner.associatedColorMaterial;
                    }
                    _currentBlinkInterval = _timeBetweenBlink;
                    _colorInverter = !_colorInverter;
                }

                if (_currentBlinkTime < 0f)
                {
                    _tileImmune = false;
                    _meshRenderer.material = _owner == null ? _defaultTileMat : _owner.associatedColorMaterial;
                }
            }
        }

        public void SetCoordinate(int z, int x)
        {
            X = x;
            Z = z;
        }

        public void SetOwner(Player player, bool withAnim = false)
        {
            if (_tileImmune == false)
            {
                _previousOwner = _owner;
                _owner = player;
                if (withAnim == true)
                {
                    _currentBlinkTime = _blinkTime;
                    _colorInverter = false;
                    _tileImmune = true;
                }
                if (player != null)
                {
                    _meshRenderer.material = player.associatedColorMaterial;
                }
                else
                {
                    _meshRenderer.material = _defaultTileMat;
                }
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
