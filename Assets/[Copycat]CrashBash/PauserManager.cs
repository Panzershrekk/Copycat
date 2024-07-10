using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauserManager : MonoBehaviour
{
    public static bool isPaused = false;
    public static PauserManager Instance { get; private set; }

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

    public void TogglePause(bool pause)
    {
        if (pause == false)
        {
            isPaused = false;
            Time.timeScale = 1.0f;
        }
        else
        {
            isPaused = true;
            Time.timeScale = 0.0f;
        }
    }
}
