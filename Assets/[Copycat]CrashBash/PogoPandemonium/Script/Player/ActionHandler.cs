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
        public Sequence JumpSequence { get; private set; }
        //% of frame where the model is actually ascending
        [SerializeField] float _jumpAscendFrameInPercent = 0.213f;
        [SerializeField] EventReference _jumpSound;
        private Pogotile nextPogoTile = null;
        private Vector3 destination;

        public void ProcessDirection(Player player, MoveDirection moveDirection)
        {
            if (Arena.Instance != null)
            {
                Pogotile playerCurrentPogotTile = player.CurrentStandingPogoTile;
                nextPogoTile = null;
                destination = Vector3.zero;
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
                destination = nextPogoTile.transform.position + new Vector3(0.5f, 0, 0.5f);
                if (player.HasLost() == false)
                {
                    FMODUtilities.PlaySoundOneShot(_jumpSound);
                }
                //JumpTween = transform.DOJump(destination, 0.4f, 1, player.CurrentTickMoveSpeed * 0.9f, false).OnComplete(() => FinishMovement(player, nextPogoTile));

                float sequenceTime = player.CurrentTickMoveSpeed * 1f;
                JumpSequence = DOTween.Sequence();

                float ascendStartTime = sequenceTime * 0.15f;
                float descendStartTime = sequenceTime * 0.67f;

                Vector3 controlPoint = (this.transform.position + destination) / 2;
                controlPoint.y += 1.5f;  // Augmenter Y pour créer l'arc

                Vector3 midPoint1 = Vector3.Lerp(this.transform.position, destination, 0.33f);
                Vector3 midPoint2 = Vector3.Lerp(this.transform.position, destination, 0.66f);
                midPoint1.y += 1f;
                midPoint2.y += 1f;

                Vector3[] path = new Vector3[] { transform.position, midPoint1, controlPoint, midPoint2, destination };
                JumpSequence.AppendInterval(ascendStartTime);
                JumpSequence.Append(transform.DOPath(path, descendStartTime - ascendStartTime, PathType.CatmullRom)
                    .SetEase(Ease.OutQuad));
                JumpSequence.AppendInterval(sequenceTime * 0.67f - descendStartTime);
                JumpSequence.Append(transform.DOMoveY(0, sequenceTime - descendStartTime).SetEase(Ease.InQuad));
                JumpSequence.Play();
                JumpSequence.OnComplete(() => FinishMovement(player, nextPogoTile));
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
            JumpSequence?.Kill();
        }
    }
}