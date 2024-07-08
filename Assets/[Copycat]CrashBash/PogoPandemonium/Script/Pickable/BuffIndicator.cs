using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace PogoPandemonium
{
    public class BuffIndicator : MonoBehaviour
    {
        private Tween _tweenInstance;


        void Start()
        {
            _tweenInstance = transform.DORotate(new Vector3(0, 360, 0), 2f, RotateMode.FastBeyond360).SetLoops(-1).SetEase(Ease.Linear);
        }

        void OnDestroy()
        {
            if (_tweenInstance != null)
            {
                _tweenInstance.Kill();
            }
        }
    }
}