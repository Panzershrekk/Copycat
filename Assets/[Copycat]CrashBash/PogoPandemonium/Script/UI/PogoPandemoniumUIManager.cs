using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

namespace PogoPandemonium
{
    public class PogoPandemoniumUIManager : MonoBehaviour
    {
        public static PogoPandemoniumUIManager Instance { get; private set; }
        [SerializeField] private GameObject _worldCanvas;
        [SerializeField] private GameObject _scorePrefab;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }
        }

        public void DisplayScore(Vector3 position, int score)
        {
            GameObject scoreTextInstance = Instantiate(_scorePrefab, position + new Vector3(0, 0.5f, 0), Quaternion.identity, _worldCanvas.transform);
            TMP_Text text = scoreTextInstance.GetComponent<TMP_Text>();
            if (text != null)
            {
                text.text = score.ToString();
            }
            scoreTextInstance.transform.DOMoveY(8, 3.5f).SetEase(Ease.InOutElastic).OnComplete(() => Destroy(scoreTextInstance));
        }
    }
}