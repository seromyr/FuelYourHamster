using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Constants;

public class UI_PlayGame : MonoBehaviour, IAudible
{
    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(NewGame);
        SoundSetup();
    }

    public void NewGame()
    {
        GameManager.main.ShowHowToPlay();
        PlaySound(SoundController.main.SoundLibrary[0]);
    }

    #region Interfaces Implementation
    private AudioSource soundPlayer;
    public void SoundSetup()
    {
        soundPlayer = SoundController.main.CreateASoundPlayer(transform);
    }

    public void PlaySound(AudioClip sound)
    {
        SoundController.main.PlaySound(soundPlayer, sound);
    }
    #endregion
}
