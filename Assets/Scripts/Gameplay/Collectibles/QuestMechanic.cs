using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestMechanic : MonoBehaviour
{
    private void Start()
    {
        Player.main.Mechanic.OnCollisionWithCollectible += OnCollected;
    }

    public void OnCollected(object sender, CollectibleType e)
    {
        if (e.hashCode == gameObject.name)
        {
            Destroy(gameObject);
            Debug.LogError("Collected quest item: " + gameObject.name);
            QuestController.main.LoadNextQuestCollectible();
            QuestController.main.AddCollectedCharacter(gameObject.name[0].ToString());
            QuestController.main.ProgressQuest();
            Player.main.ChangeCollisionColor(Color.white);
        }
    }

    private void OnDestroy()
    {
        Player.main.Mechanic.OnCollisionWithCollectible -= OnCollected;
    }
}
