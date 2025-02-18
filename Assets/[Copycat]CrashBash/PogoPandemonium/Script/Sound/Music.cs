using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class Music : MonoBehaviour
{
    [SerializeField] EventReference _music;
    FMOD.Studio.EventInstance _eventInstance;

    void Start()
    {
        PlayMusic();
    }

    private void PlayMusic()
    {
        if (!_music.IsNull)
        {
            _eventInstance = RuntimeManager.CreateInstance(_music);
            _eventInstance.start();
        }
    }

    public void SetGameValue(float value)
    {
        _eventInstance.setParameterByName("Game", value);
    }
}
