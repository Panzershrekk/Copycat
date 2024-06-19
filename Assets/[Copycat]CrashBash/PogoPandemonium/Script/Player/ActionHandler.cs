using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PangoPandemonium
{
    public class ActionHandler : MonoBehaviour
    {
        public void ProcessDirection(Player player, MoveDirection moveDirection)
        {
            Pogotile nextPogoTile = null;
            Pogotile playerCurrentPogotTile = player.CurrentStandingPogoTile;

            if (moveDirection == MoveDirection.North)
            {
                nextPogoTile = Arena.Instance.GetPogotileAtCoordinate(playerCurrentPogotTile.Z + 1, playerCurrentPogotTile.X);
            }
            else if (moveDirection == MoveDirection.East)
            {
                nextPogoTile = Arena.Instance.GetPogotileAtCoordinate(playerCurrentPogotTile.Z, playerCurrentPogotTile.X + 1);
            }
            else if (moveDirection == MoveDirection.South)
            {
                nextPogoTile = Arena.Instance.GetPogotileAtCoordinate(playerCurrentPogotTile.Z - 1, playerCurrentPogotTile.X);

            }
            else if (moveDirection == MoveDirection.West)
            {
                nextPogoTile = Arena.Instance.GetPogotileAtCoordinate(playerCurrentPogotTile.Z, playerCurrentPogotTile.X - 1);
            }
            if (nextPogoTile == null)
            {
                nextPogoTile = player.CurrentStandingPogoTile;
            }
            //player.SetCurrentPogotile(nextPogoTile);
        }
    }
}