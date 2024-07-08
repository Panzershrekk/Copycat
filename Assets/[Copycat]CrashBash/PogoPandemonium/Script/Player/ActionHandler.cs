using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using FMODUnity;
using UnityEngine;

namespace PogoPandemonium
{
    public class ActionHandler : MonoBehaviour
    {
        public Tween JumpTween { get; private set; }
        [SerializeField] EventReference _jumpSound;

        public void ProcessDirection(Player player, MoveDirection moveDirection)
        {
            if (Arena.Instance != null)
            {
                Pogotile nextPogoTile = null;
                Pogotile playerCurrentPogotTile = player.CurrentStandingPogoTile;

                if (moveDirection == MoveDirection.North)
                {
                    nextPogoTile = Arena.Instance.GetPogotileAtCoordinate(playerCurrentPogotTile.Z + 1, playerCurrentPogotTile.X);
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                }
                else if (moveDirection == MoveDirection.East)
                {
                    nextPogoTile = Arena.Instance.GetPogotileAtCoordinate(playerCurrentPogotTile.Z, playerCurrentPogotTile.X + 1);
                    transform.rotation = Quaternion.Euler(0, 90, 0);
                }
                else if (moveDirection == MoveDirection.South)
                {
                    nextPogoTile = Arena.Instance.GetPogotileAtCoordinate(playerCurrentPogotTile.Z - 1, playerCurrentPogotTile.X);
                    transform.rotation = Quaternion.Euler(0, 180, 0);
                }
                else if (moveDirection == MoveDirection.West)
                {
                    nextPogoTile = Arena.Instance.GetPogotileAtCoordinate(playerCurrentPogotTile.Z, playerCurrentPogotTile.X - 1);
                    transform.rotation = Quaternion.Euler(0, 270, 0);
                }
                if (nextPogoTile != null && nextPogoTile.OccupiedByPlayer == true)
                {
                    nextPogoTile = null;
                }
                if (nextPogoTile == null)
                {
                    nextPogoTile = player.CurrentStandingPogoTile;
                }
                playerCurrentPogotTile.SetOccupiedByPlayer(false);
                nextPogoTile.SetOccupiedByPlayer(true);
                Vector3 destination = nextPogoTile.transform.position + new Vector3(0.5f, 0, 0.5f);
                FMODUtilities.PlaySoundOneShot(_jumpSound);
                JumpTween = transform.DOJump(destination, 0.4f, 1, player.CurrentTickMoveSpeed * 0.9f, false).OnComplete(() => FinishMovement(player, nextPogoTile));
            }
        }

        private void FinishMovement(Player player, Pogotile pogotile)
        {
            player.SetCurrentPogotile(pogotile);
            if (pogotile.OccupiedByObject == true)
            {
                pogotile.PickableOnTile.Pick(player);
                pogotile.SetOccupiedByObject(false, null);
            }
        }

        private void OnDestroy()
        {
            if (JumpTween != null)
            {
                JumpTween.Kill();
            }
        }
    }
}