using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Constants;

public class UI_PlayGame : MonoBehaviour
{
    private Button button;
    private AudioSource soundPlayer;

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(NewGame);

        soundPlayer = SoundController.main.CreateASoundPlayer(transform);
    }

    public void NewGame()
    {
        GameManager.main.ShowHowToPlay();
        PlaySound();
    }

    private void PlaySound()
    {
        SoundController.main.PlaySound(soundPlayer, SoundController.main.SoundLibrary[0]);
    }
}
