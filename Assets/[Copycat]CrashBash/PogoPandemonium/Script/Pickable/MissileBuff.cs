using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PogoPandemonium
{
    public class MissileBuff : Buff, IActiveBonus
    {
        public MissileBuff(GameObject indicatorPrefab) : base(indicatorPrefab) { }

        public override void Apply(Player player)
        {
            base.Apply(player);
        }

        public override void Remove(Player player)
        {
            base.Remove(player);
        }

        public void Use(Player player)
        {
            Debug.Log("DEEZ NUTS");
            player.RemoveBuff();
        }
    }
}