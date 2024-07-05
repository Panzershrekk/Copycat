using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace PogoPandemonium
{
    public class SpeedyShoes : MonoBehaviour, IPickable
    {
        [SerializeField] GameObject _indicator;
        private Tween _tweenInstance;
        private Tween _appearTween;
        private float _yBasePosition;

        public void Start()
        {
            _yBasePosition = this.transform.position.y;
            transform.position += new Vector3(0, 6, 0);
            _appearTween = transform.DOMove(new Vector3(this.transform.position.x, _yBasePosition, this.transform.position.z), 0.5f).SetEase(Ease.OutCubic);
            _tweenInstance = transform.DORotate(new Vector3(0, 360, 0), 2f, RotateMode.FastBeyond360).SetLoops(-1).SetEase(Ease.Linear);
        }

        public void Pick(Player player)
        {
            player.ApplyBuff(new SpeedyShoesBuff(_indicator));
            Arena.Instance.RemoveShoesFromItsList(this);
            Destroy(this.gameObject);
        }

        private void OnDestroy()
        {
            if (_tweenInstance != null)
            {
                _tweenInstance.Kill();
                _appearTween.Kill();
            }
        }
    }
}