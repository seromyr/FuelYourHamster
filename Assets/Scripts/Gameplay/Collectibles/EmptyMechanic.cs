using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyMechanic : MonoBehaviour
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
            //Debug.Log("Collect 1 emptiness");
            Player.main.ChangeCollisionColor(Color.white);
        }
    }

    private void OnDestroy()
    {
        //Debug.Log("Coin: " + transform.name + " destroyed");
        Player.main.Mechanic.OnCollisionWithCollectible -= OnCollected;
    }
}
