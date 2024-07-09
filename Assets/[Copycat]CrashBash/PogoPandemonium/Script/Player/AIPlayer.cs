using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PogoPandemonium
{
    public class AIPlayer : Player
    {
        [Header("AI Specific")]
        [SerializeField] int _variance = 20; //The bigger the variance, the more random it gets
        [SerializeField] int _weightForCrateDistance = 50; //The bigger it is, the stronger the IA will move toward boxes

        protected override void Update()
        {
            if (_currentTickMove < 0)
            {
                List<Pogotile> pogotiles = Arena.Instance.GetAdjacentTileOfTile(CurrentStandingPogoTile);
                int bestWeight = 0;
                Pogotile selectedTile = null;
                foreach (Pogotile pogotile in pogotiles)
                {
                    int weight = 0;
                    Player owner = pogotile.GetOwner();
                    IPickable pickableOnTile = pogotile.PickableOnTile;
                    Pogotile pointCrateTile = Arena.Instance.GetClosestCrateFromPogotile(CurrentStandingPogoTile);
                    MoveDirection directionOfBox = Arena.Instance.GetRelativeDirectionFromTile(CurrentStandingPogoTile, pointCrateTile);
                    MoveDirection directionOfTile = Arena.Instance.GetRelativeDirectionFromTile(CurrentStandingPogoTile, pogotile);
                    int distanceFromBox = Arena.Instance.GetDistanceFromTile(CurrentStandingPogoTile, pointCrateTile);
                    if (directionOfBox == directionOfTile)
                    {
                        weight += 10 + (distanceFromBox != 0 ? Mathf.Abs(_weightForCrateDistance / distanceFromBox) : 0);
                    }
                    if (pickableOnTile != null)
                    {
                        weight += 10;
                        if (pickableOnTile is Missile)
                        {
                            if (_currentBuff is SpeedyShoesBuff)
                            {
                                weight -= 20 + Random.Range(0, _variance);
                            }
                            else
                            {
                                weight += 20 + Random.Range(0, _variance);
                            }
                        }
                        if (pickableOnTile is ArrowBonus)
                        {
                            weight += 20 + Random.Range(0, _variance);
                        }
                        if (pickableOnTile is SpeedyShoes)
                        {
                            weight += 40 + Random.Range(0, _variance);
                        }
                        if (pickableOnTile is PointCrate)
                        {
                            weight += 1000;
                        }
                    }
                    if (owner == null)
                    {
                        weight += 10 + Random.Range(0, _variance);
                    }
                    if (owner == this)
                    {
                        weight += -5 + Random.Range(0, _variance);
                    }
                    if (owner != null && owner != this)
                    {
                        weight += 10 + Random.Range(0, _variance);
                    }
                    if (pogotile.OccupiedByPlayer == true)
                    {
                        weight += -100000;
                    }
                    if (weight > bestWeight)
                    {
                        bestWeight = weight;
                        selectedTile = pogotile;
                    }
                }
                if (Arena.Instance.IsAlignedWithPlayer(CurrentStandingPogoTile, _currenMoveDirection) == true && _currentBuff is MissileBuff)
                {
                    UseActive();
                }
                if (selectedTile != null)
                {
                    _currenMoveDirection = Arena.Instance.GetRelativeDirectionFromTile(CurrentStandingPogoTile, selectedTile);
                }
                else
                {
                    _currenMoveDirection = MoveDirection.None;
                }
            }
            base.Update();
        }
    }
}