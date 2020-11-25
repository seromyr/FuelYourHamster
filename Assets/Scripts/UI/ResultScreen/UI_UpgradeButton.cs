using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Constants;

public class UI_UpgradeButton : MonoBehaviour
{
    private Button button;
    private Text text;
    private AudioSource soundPlayer;

    private void Start()
    {
        button = GetComponent<Button>();
        GameManager.main.OnGameEnd += SwitchButtonToEndGame;
        GameManager.main.OnGamePlay += SwitchButtonToUpgrade;

        text = transform.GetComponentInChildren<Text>();

        soundPlayer = SoundController.main.CreateASoundPlayer(transform);
    }

    private void ShowUpgrade()
    {
        GameManager.main.PauseGame();
        PlaySound();
    }

    private void GotoMainMenu()
    {
        GameManager.main.GoToTheMainMenu();
        PlaySound();
    }

    private void SwitchButtonToEndGame(object sender, EventArgs e)
    {
        text.text = "Main Menu";
        button.onClick.AddListener(GotoMainMenu);
        button.onClick.RemoveListener(ShowUpgrade);
    }

    private void SwitchButtonToUpgrade(object sender, EventArgs e)
    {
        text.text = "Upgrade";
        button.onClick.AddListener(ShowUpgrade);
        button.onClick.RemoveListener(GotoMainMenu);
    }

    private void PlaySound()
    {
        SoundController.main.PlaySound(soundPlayer, SoundController.main.SoundLibrary[0]);
    }
}
