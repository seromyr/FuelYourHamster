using UnityEngine;

interface IAudible
{
    void SoundSetup();

    void PlaySound(AudioClip sound);
}