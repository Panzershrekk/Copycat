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

        public void UpdatePointText(int value)
        {
            _pointText.text = value.ToString();
            _currentValue = 0;
        }
    }
}