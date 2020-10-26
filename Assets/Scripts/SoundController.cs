using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;
using UnityEngine.SceneManagement;

public class SoundController : MonoBehaviour
{
    public List<AudioClip> playList;

    private AudioSource audioSource;
    private string soundComponent, musicComponent;
    public string SoundComponent { get { return soundComponent; } }
    public string MusicComponent { get { return musicComponent; } }

    private AudioState soundState, musicState;
    public AudioState SoundState { get { return soundState; } }
    public AudioState MusicState { get { return musicState; } }

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        soundComponent = AudioComponent.SOUND;
        musicComponent = AudioComponent.MUSIC;

        soundState = AudioState.On;
        musicState = AudioState.On;

        if (playList.Count > 0)
        {
            audioSource.clip = playList[0];
        }

        audioSource.Play();
        audioSource.loop = true;

    }

    private void OnLevelWasLoaded()
    {
        SongAssign();
    }
    
    private void SongAssign()
    {
        if (SceneManager.GetActiveScene().name == SceneName.MAINMENU)
        {
            if (playList.Count > 0)
            {
                audioSource.clip = playList[0];
            }
        }
        else if (SceneManager.GetActiveScene().name == SceneName.GAME)
        {
            if (playList.Count > 1)
            {
                audioSource.clip = playList[1];
            }
        }

        if (playList != null) audioSource.Play();
    }

    public void MusicSwitch()
    {
        switch (musicState)
        {
            case AudioState.On:
                musicState = AudioState.Off;
                audioSource.volume = 0;
                break;
            case AudioState.Off:
                musicState = AudioState.On;
                audioSource.volume = 1;
                break;
        }
    }

    public void SoundSwitch()
    {
        soundState = soundState == AudioState.Off ? AudioState.On : AudioState.Off;
    }
}
