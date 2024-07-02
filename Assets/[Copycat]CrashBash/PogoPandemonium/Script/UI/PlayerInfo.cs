using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace PogoPandemonium
{
    public class PlayerInfo : MonoBehaviour
    {
        [SerializeField] private TMP_Text _pointText;
        private int _currentValue = 0;
        private int _startValue = 0;
        private int _endValue = 0;

        private float _duration = 0.5f;
        private Coroutine _currentRoutine;

        public void UpdatePointText(int value)
        {
            _startValue = _currentValue;
            _endValue = value;
            if (_currentRoutine != null)
            {
                StopCoroutine(_currentRoutine);
            }
            _currentRoutine = StartCoroutine(UpdatePoints());
        }

        private IEnumerator UpdatePoints()
        {
            float elapsed = 0f;

            while (elapsed < 1)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / _duration);
                _currentValue = Mathf.RoundToInt(Mathf.Lerp(_startValue, _endValue, t));
                _pointText.text = _currentValue.ToString("D3");
                yield return null;
            }
            _pointText.text = _endValue.ToString("D3");
        }
    }
}