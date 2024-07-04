using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PogoPandemonium
{
    public abstract class Buff : IBuff
    {
        private GameObject _indicator;

        public Buff(GameObject indicatorPrefab)
        {
            _indicator = GameObject.Instantiate(indicatorPrefab);
            _indicator.SetActive(false);
        }

        public virtual void Apply(Player player)
        {
            _indicator.transform.SetParent(player.GetBuffIndicatorParent());
            _indicator.transform.localPosition = new Vector3(0, 0, 0);
            _indicator.SetActive(true);
        }

        public virtual void Remove(Player player)
        {
            _indicator.SetActive(false);
            GameObject.Destroy(_indicator.gameObject);
        }
    }
}