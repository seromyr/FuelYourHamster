using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Constants;

public class UI_UnPauseGameButton : MonoBehaviour
{
    private Button button;
    private AudioSource soundPlayer;

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(ContinueGame);

        soundPlayer = SoundController.main.CreateASoundPlayer(transform);
    }
    public void ContinueGame()
    {
        GameManager.main.UnPauseGame();
        PlaySound();
    }

    private void PlaySound()
    {
        SoundController.main.PlaySound(soundPlayer, SoundController.main.SoundLibrary[0]);
    }
}
