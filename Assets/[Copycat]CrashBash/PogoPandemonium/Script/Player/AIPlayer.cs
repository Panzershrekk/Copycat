using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PogoPandemonium
{
    public class AIPlayer : Player
    {

        // Update is called once per frame
        protected override void Update()
        {
            //Random for now
            if (_currentTickMove < 0)
            {
                List<Pogotile> pogotiles = Arena.Instance.GetAdjacentTileOfTile(CurrentStandingPogoTile);
                int bestWeight = 0;
                Pogotile selectedTile = null;
                foreach (Pogotile pogotile in pogotiles)
                {
                    int weight = 0;
                    Player owner = pogotile.GetOwner();
                    if (owner == null)
                    {
                        weight = 10 + Random.Range(0, 20);
                    }
                    if (owner == this)
                    {
                        weight = -10 + Random.Range(0, 25);
                    }
                    if (owner != null && owner != this)
                    {
                        weight = 20 + Random.Range(-10, 10);
                    }
                    if (pogotile.OccupiedByPlayer == true)
                    {
                        weight = -100;
                    }
                    if (weight > bestWeight)
                    {
                        bestWeight = weight;
                        selectedTile = pogotile;
                    }
                }
                if (selectedTile != null)
                {
                    _currenMoveDirection = Arena.Instance.GetRelativeDirectionFromTile(CurrentStandingPogoTile, selectedTile);
                }
                else
                {
                    _currenMoveDirection = MoveDirection.None;
                }
                //_currenMoveDirection = MoveDirection.South;
                Debug.Log("_currenMoveDirection " + _currenMoveDirection);
            }
            base.Update();
        }
    }
}