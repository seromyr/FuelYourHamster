using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestMechanic : MonoBehaviour, IAudible
{
    private void Start()
    {
        Player.main.Mechanic.OnCollisionWithCollectible += OnCollected;
        SoundSetup();
    }

    public void OnCollected(object sender, CollectibleType e)
    {
        if (e.hashCode == gameObject.name)
        {
            Destroy(gameObject);
            Debug.Log("Collected quest item: " + gameObject.name);
            QuestController.main.LoadNextQuestCollectible();
            QuestController.main.AddCollectedCharacter(gameObject.name[0].ToString());
            QuestController.main.ProgressQuest();
            Player.main.ChangeCollisionColor(Color.white);
            UI_Gameplay_Mechanic.main.HighlightQuestImageOnHUD();
            PlaySound(SoundController.main.SoundLibrary[2]);
        }
    }

    private void OnDestroy()
    {
        Player.main.Mechanic.OnCollisionWithCollectible -= OnCollected;
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
