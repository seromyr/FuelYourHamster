using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest : MutatedCollectible
{
    public Quest() : base ()
    {
        UpdateMechanic();
    }

    private void UpdateMechanic()
    {
        _body.name = _name = "Quest Item";
        _body.transform.position = Vector3.zero;
        Object.Destroy(_body.GetComponent<MutatationMechanic>());
        _body.AddComponent<QuestMechanic>();
        _avatar.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Collectable Sprites/Quest/B_");
    }
}
