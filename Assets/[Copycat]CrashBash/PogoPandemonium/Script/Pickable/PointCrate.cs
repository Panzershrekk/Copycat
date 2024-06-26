using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using PogoPandemonium;
using UnityEngine;

public class PointCrate : MonoBehaviour, IPickable
{
    public void Start()
    {
        this.transform.localScale = new Vector3(1, 0, 1);
        transform.DOScale(new Vector3(1, 1, 1), 0.2f);
    }

    public void Pick(Player player)
    {
        Arena.Instance.ValidatePointForPlayer(player, this);
        Destroy(this.gameObject);
    }
}
