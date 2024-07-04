using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace PogoPandemonium
{
    public class Missile : MonoBehaviour, IPickable
    {
        [SerializeField] GameObject _indicator;
        private Tween _tweenInstance;

        public void Start()
        {
            _tweenInstance = transform.DORotate(new Vector3(0, 360, 0), 2f, RotateMode.FastBeyond360).SetLoops(-1).SetEase(Ease.Linear);
        }

        public void Pick(Player player)
        {
            player.ApplyBuff(new MissileBuff(_indicator));
            Arena.Instance.RemoveMissileFromItsList(this);
            Destroy(this.gameObject);
        }

        private void OnDestroy()
        {
            if (_tweenInstance != null)
            {
                _tweenInstance.Kill();
            }
        }
    }
}