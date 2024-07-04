using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PogoPandemonium
{
    public class MissileBuff : Buff, IActiveBonus
    {
        private MissileDamager _missileDamager;

        public MissileBuff(GameObject indicatorPrefab, MissileDamager damager) : base(indicatorPrefab) { 
            _missileDamager = damager;
        }

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
            MissileDamager missileDamager = GameObject.Instantiate(_missileDamager, player.transform.position + new Vector3(0, 0.5f, 0), player.transform.rotation, null);
            missileDamager.Setup(player);
            player.RemoveBuff();
        }
    }
}