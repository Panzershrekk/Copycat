using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

namespace PogoPandemonium
{
    public abstract class Pickable : MonoBehaviour, IPickable
    {
        [SerializeField] EventReference _pickUpSound;

        public abstract void Pick(Player player);

        protected void PlayPickUpSound()
        {
            if (!_pickUpSound.IsNull)
            {
                FMODUtilities.PlaySoundOneShot(_pickUpSound);
            }
        }
    }
}
