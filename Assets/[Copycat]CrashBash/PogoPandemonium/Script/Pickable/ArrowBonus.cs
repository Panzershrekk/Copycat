using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace PogoPandemonium
{
    public class ArrowBonus : MonoBehaviour, IPickable
    {
        [SerializeField] private float _switchTime = 0.5f;
        private MoveDirection _facingDirection = MoveDirection.North;
        private float _currentSwitchTime = 0;

        // Start is called before the first frame update
        void Start()
        {
            _facingDirection = (MoveDirection)Random.Range(1, 5);
            transform.rotation = Quaternion.Euler(GetRotationFromDirection(_facingDirection));
            _currentSwitchTime = 0.5f;
        }

        // Update is called once per frame
        void Update()
        {
            _currentSwitchTime -= Time.deltaTime;
            if (_currentSwitchTime < 0)
            {
                SwitchToDirection(_facingDirection);
                _currentSwitchTime = _switchTime;
            }
        }

        public void Pick(Player player)
        {
            Arena.Instance.FillTilesFromTileAndDirectionForPlayer(player.CurrentStandingPogoTile, _facingDirection, player);
            Destroy(this.gameObject);
        }

        public void SwitchToDirection(MoveDirection moveDirection)
        {
            List<MoveDirection> moveDirections = GetAdjacentDirection(moveDirection);
            _facingDirection = moveDirections[Random.Range(0, moveDirections.Count)];
            transform.DORotate(GetRotationFromDirection(_facingDirection), 0.2f, RotateMode.Fast);
        }

        private List<MoveDirection> GetAdjacentDirection(MoveDirection direction)
        {
            List<MoveDirection> moveDirections = new List<MoveDirection>();

            if (direction == MoveDirection.South || direction == MoveDirection.North)
            {
                moveDirections.Add(MoveDirection.East);
                moveDirections.Add(MoveDirection.West);
            }
            if (direction == MoveDirection.West || direction == MoveDirection.East)
            {
                moveDirections.Add(MoveDirection.North);
                moveDirections.Add(MoveDirection.South);
            }
            return moveDirections;
        }

        private Vector3 GetRotationFromDirection(MoveDirection direction)
        {
            if (direction == MoveDirection.North)
            {
                return new Vector3(0, 0, 0);
            }
            if (direction == MoveDirection.East)
            {
                return new Vector3(0, 90, 0);
            }
            if (direction == MoveDirection.South)
            {
                return new Vector3(0, 180, 0);
            }
            if (direction == MoveDirection.West)
            {
                return new Vector3(0, 270, 0);
            }
            return new Vector3();
        }
    }
}