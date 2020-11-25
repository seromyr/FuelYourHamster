using Constants;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SoundOnOff : MonoBehaviour
{
    private SoundController soundController;
    //private AudioState audioState;
    private Button button;
    private Text btnText;
    private string audioComponent;

    private void Start()
    {
        soundController = GameObject.Find("SoundController").GetComponent<SoundController>();
        button = GetComponent<Button>();
        btnText = GetComponentInChildren<Text>();
        audioComponent = transform.name;

        if (audioComponent == soundController.MusicComponent)
        {
            btnText.text = audioComponent + " " + soundController.MusicState;
        }
        else if (audioComponent == soundController.SoundComponent)
        {
            btnText.text = audioComponent + " " + soundController.SoundState;
        }

        button.onClick.AddListener(StateToggle);
    }

    private void StateToggle()
    {
        switch (audioComponent)
        {
            case AudioComponent.MUSIC:
                soundController.SwitchMusicState();
                btnText.text = audioComponent + " " + soundController.MusicState;
                break;
            case AudioComponent.SOUND:
                soundController.SwitchSoundState();
                btnText.text = audioComponent + " " + soundController.SoundState;
                break;
        }
    }
}
