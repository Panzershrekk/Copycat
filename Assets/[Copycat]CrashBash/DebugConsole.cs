using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DebugConsole : MonoBehaviour
{
    public TMP_Text debugText; 

    public static DebugConsole Instance { get; private set; }

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

    void Start()
    {
        //Application.targetFrameRate = 10;
    }

    public void ChangeDebugMessage(string message)
    {
        debugText.text = message;
    }
}
