using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffeeBean : MutatedCollectible
{
    public CoffeeBean() : base()
    {
        UpdateMechanic();
    }

    private void UpdateMechanic()
    {
        _body.name = _name = "CoffeBean";
        _body.transform.position = Vector3.zero;
        Object.Destroy(_body.GetComponent<MutationAppearance>());
        _avatar.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Collectable Sprites/CoffeeBean/CoffeeBean");
    }
}
