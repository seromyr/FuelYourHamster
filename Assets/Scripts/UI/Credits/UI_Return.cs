using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Return : MonoBehaviour
{
    private Button button;
    private AudioSource soundPlayer;

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(Return);

        soundPlayer = SoundController.main.CreateASoundPlayer(transform);
    }
    public void Return()
    {
        UI_MainMenu.main.FadeIn(1);
        UI_Credits.main.FadeOut();
        UI_Credits.main.SelfScroller.ScrollReset();
        PlaySound();
    }

    private void PlaySound()
    {
        SoundController.main.PlaySound(soundPlayer, SoundController.main.SoundLibrary[0]);
    }
}
