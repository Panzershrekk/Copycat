using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class Music : MonoBehaviour
{
    [SerializeField] EventReference _music;

    void Start()
    {
        PlayMusic();
    }

    private void PlayMusic()
    {
        if (!_music.IsNull)
        {
            FMOD.Studio.EventInstance eventInstance = RuntimeManager.CreateInstance(_music);
            eventInstance.start();
            eventInstance.release();
        }
    }
}
