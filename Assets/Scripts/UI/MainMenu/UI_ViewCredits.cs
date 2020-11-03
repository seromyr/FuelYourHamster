using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ViewCredits : MonoBehaviour
{
    private Button button;
    private AudioSource soundPlayer;

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(ViewCredits);

        soundPlayer = SoundController.main.CreateASoundPlayer(transform);
    }
    public void ViewCredits()
    {
        UI_MainMenu.main.FadeOut(1);
        UI_Credits.main.FadeIn();
        UI_Credits.main.SelfScroller.Scroll();
        PlaySound();
    }

    private void PlaySound()
    {
        SoundController.main.PlaySound(soundPlayer, SoundController.main.SoundLibrary[0]);
    }
}
