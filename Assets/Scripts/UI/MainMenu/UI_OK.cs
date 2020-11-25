using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_OK : MonoBehaviour
{
    private Button button;
    private AudioSource soundPlayer;

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OK);

        soundPlayer = SoundController.main.CreateASoundPlayer(transform);
    }

    public void OK()
    {
        GameManager.main.NewGame();
        PlaySound();
    }

    private void PlaySound()
    {
        SoundController.main.PlaySound(soundPlayer, SoundController.main.SoundLibrary[0]);
    }
}
