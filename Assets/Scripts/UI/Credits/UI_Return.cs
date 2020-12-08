using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Return : MonoBehaviour, IAudible
{
    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(Return);

        SoundSetup();
    }
    public void Return()
    {
        UI_MainMenu.main.FadeIn(1);
        UI_Credits.main.FadeOut();
        UI_Credits.main.SelfScroller.ScrollReset();
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
