using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace PogoPandemonium
{
    public class PogoPandemoniumUIManager : MonoBehaviour
    {
        public static PogoPandemoniumUIManager Instance { get; private set; }
        public GameObject worldCanvas;
        public GameObject scorePrefab;

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
            GameObject scoreTextInstance = Instantiate(scorePrefab, position, Quaternion.identity, worldCanvas.transform);
            //scoreTextInstance.text = "+" + points.ToString();
        }
    }
}