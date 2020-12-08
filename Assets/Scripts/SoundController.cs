using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;
using UnityEngine.SceneManagement;

public class SoundController : MonoBehaviour
{
    public static SoundController main;

    [SerializeField]
    private List<AudioClip> musicLibrary, soundLibrary;
    public List<AudioClip> SoundLibrary { get { return soundLibrary; } }

    private AudioSource musicSource;

    public string SoundComponent { get { return AudioComponent.SOUND; } }
    public string MusicComponent { get { return AudioComponent.MUSIC; } }

    private AudioState soundState, musicState;
    public AudioState SoundState { get { return soundState; } }
    public AudioState MusicState { get { return musicState; } }

    private GameObject sfxAudioSourceContainer;

    void Awake()
    {
        Singletonizer();

        GameObject musicPlayer = new GameObject("MusicPlayer");
        musicPlayer.transform.SetParent(transform);
        musicSource = musicPlayer.AddComponent<AudioSource>();

        soundState = AudioState.On;
        musicState = AudioState.On;

        musicLibrary = new List<AudioClip>();
        musicLibrary.AddRange(Resources.LoadAll<AudioClip>("SFX/Musics"));

        soundLibrary = new List<AudioClip>();
        soundLibrary.AddRange(Resources.LoadAll<AudioClip>("SFX/Sounds"));

        sfxAudioSourceContainer = new GameObject("AudioSourceContainer");
        sfxAudioSourceContainer.transform.SetParent(transform);
    }

    private void Start()
    {
        if (musicLibrary.Count > 0)
        {
            musicSource.clip = musicLibrary[0];
        }

        musicSource.volume = CONST.DEFAULT_BGM_VOLUME;
        musicSource.playOnAwake = false;
        musicSource.loop = true;
    }

    private void Singletonizer()
    {
        if (main == null)
        {
            DontDestroyOnLoad(gameObject);
            Debug.Log("Sound Controller created");
            main = this;
        }
        else if (main != this)
        {
            Destroy(gameObject);
        }
    }
    
    public void SwitchBGM()
    {
        if (SceneManager.GetActiveScene().name == SceneName.MAINMENU)
        {
            if (musicLibrary.Count > 0)
            {
                musicSource.clip = musicLibrary[0];
            }
        }
        else if (SceneManager.GetActiveScene().name == SceneName.GAME)
        {
            if (musicLibrary.Count > 1)
            {
                musicSource.clip = musicLibrary[1];
            }
        }
    }

    public void SwitchMusicState()
    {
        switch (musicState)
        {
            case AudioState.On:
                musicState = AudioState.Off;
                musicSource.volume = 0;
                break;
            case AudioState.Off:
                musicState = AudioState.On;
                musicSource.volume = CONST.DEFAULT_BGM_VOLUME;
                break;
        }
    }

    public void PlayBGM()
    {
        if (musicLibrary != null) musicSource.Play();
    }

    public void StopBGM()
    {
        if (musicSource.isPlaying) musicSource.Stop();
    }

    public void SwitchSoundState()
    {
        switch (soundState)
        {
            case AudioState.On:
                soundState = AudioState.Off;
                for (int i = 0; i < sfxAudioSourceContainer.transform.childCount; i++)
                {
                    sfxAudioSourceContainer.transform.GetChild(i).GetComponent<AudioSource>().volume = 0;
                }
                break;
            case AudioState.Off:
                soundState = AudioState.On;
                for (int i = 0; i < sfxAudioSourceContainer.transform.childCount; i++)
                {
                    sfxAudioSourceContainer.transform.GetChild(i).GetComponent<AudioSource>().volume = 1;
                }
                break;
        }
    }

    public AudioSource CreateASoundPlayer(Transform owner)
    {
        GameObject soundPlayer = new GameObject("SoundPlayer of " + owner.name);
        soundPlayer.transform.SetParent(sfxAudioSourceContainer.transform);

        AudioSource thisAutioSource = soundPlayer.AddComponent<AudioSource>();
        thisAutioSource.playOnAwake = false;
        thisAutioSource.loop = false;

        return thisAutioSource;
    }

    public void PlaySound(AudioSource soundPlayer,AudioClip sound)
    {
        //soundPlayer.clip = sound;
        //soundPlayer.Play();

        // Play multiple sounds at once with 1 audio source
        // Limitation: 10 - 12 sounds
        soundPlayer.PlayOneShot(sound);
    }
}
