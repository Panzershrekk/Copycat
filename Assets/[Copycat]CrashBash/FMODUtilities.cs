using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public static class FMODUtilities
{
    public static void PlaySoundOneShot(EventReference eventReference)
    {
        if (!eventReference.IsNull)
        {
            FMOD.Studio.EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
            eventInstance.start();
            eventInstance.release();
        }
    }
}
