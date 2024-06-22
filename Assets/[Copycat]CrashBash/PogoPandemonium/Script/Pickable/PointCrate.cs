using System.Collections;
using System.Collections.Generic;
using PogoPandemonium;
using UnityEngine;

public class PointCrate : MonoBehaviour, IPickable
{
    public void Pick(Player player)
    {
        Arena.Instance.ValidatePointForPlayer(player, this);
        Destroy(this.gameObject);
    }
}
